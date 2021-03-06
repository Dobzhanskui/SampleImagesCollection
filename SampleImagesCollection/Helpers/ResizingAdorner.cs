﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace SampleMVVMWPF.Helpers
{
    public class ResizingAdorner : Adorner
    {
        // Resizing adorner uses Thumbs for visual elements.  
        // The Thumbs have built-in mouse input handling.
        private Thumb topLeft, topRight, bottomLeft, bottomRight, MiddleLeft, MiddleRight, MiddleTop, MiddleBottom;

        // To store and manage the adorner's visual children.
        private VisualCollection visualChildren;

        // Initialize the ResizingAdorner.
        public ResizingAdorner(UIElement adornedElement)
          : base(adornedElement)
        {
            visualChildren = new VisualCollection(this);

            // Call a helper method to initialize the Thumbs
            // with a customized cursors.

            BuildAdornerCorner(ref topRight, Cursors.SizeNESW);//RNTP
            BuildAdornerCorner(ref topLeft, Cursors.SizeNESW);//RNTP
            BuildAdornerCorner(ref bottomLeft, Cursors.SizeNESW);//RNTP
            BuildAdornerCorner(ref bottomRight, Cursors.SizeNESW);//RNTP

            //BuildAdornerCorner(ref MiddleLeft, Cursors.SizeWE);//RNTP
            //BuildAdornerCorner(ref MiddleRight, Cursors.SizeWE);//RNTP
            //BuildAdornerCorner(ref MiddleTop, Cursors.SizeNS);//RNTP
            //BuildAdornerCorner(ref MiddleBottom, Cursors.SizeNS);//RNTP

            topRight.DragDelta += new DragDeltaEventHandler(HandleTopRight);//RNTP
            topLeft.DragDelta += new DragDeltaEventHandler(HandleTopLeft);//RNTP
            bottomLeft.DragDelta += new DragDeltaEventHandler(HandleBottomLeft);//RNTP
            bottomRight.DragDelta += new DragDeltaEventHandler(HandleBottomRight);//RNTP

            //MiddleLeft.DragDelta += new DragDeltaEventHandler(HandleMiddleLeft);//RNTP
            //MiddleRight.DragDelta += new DragDeltaEventHandler(HandleMiddleRight);//RNTP
            //MiddleTop.DragDelta += new DragDeltaEventHandler(HandleMiddleTop);//RNTP
            //MiddleBottom.DragDelta += new DragDeltaEventHandler(HandleMiddleBottom);//RNTP
        }

        //RNTP starts
        //private void HandleMiddleLeft(object sender, DragDeltaEventArgs args)
        //{
        //    var adornedElement = this.AdornedElement as FrameworkElement;
        //    var hitThumb = sender as Thumb;

        //    if (adornedElement == null || hitThumb == null) return;
        //    FrameworkElement parentElement = adornedElement.Parent as FrameworkElement;

        //    // Ensure that the Width and Height are properly initialized after the resize.
        //    EnforceSize(adornedElement);

        //    // Change the size by the amount the user drags the mouse, as long as it's larger 
        //    // than the width or height of an adorner, respectively.
        //    adornedElement.Width = Math.Max(adornedElement.Width - args.HorizontalChange, hitThumb.DesiredSize.Width);
        //}

        //private void HandleMiddleRight(object sender, DragDeltaEventArgs args)
        //{
        //    var hitThumb = sender as Thumb;

        //    if (!(this.AdornedElement is FrameworkElement adornedElement) || hitThumb == null)
        //        return;

        //    FrameworkElement parentElement = adornedElement.Parent as FrameworkElement;

        //    // Ensure that the Width and Height are properly initialized after the resize.
        //    EnforceSize(adornedElement);

        //    // Change the size by the amount the user drags the mouse, as long as it's larger 
        //    // than the width or height of an adorner, respectively.
        //    adornedElement.Width = Math.Max(adornedElement.Width + args.HorizontalChange, hitThumb.DesiredSize.Width);
        //    //adornedElement.Height = Math.Max(args.VerticalChange + adornedElement.Height, hitThumb.DesiredSize.Height);
        //}
        //private void HandleMiddleTop(object sender, DragDeltaEventArgs args)
        //{
        //    Thumb hitThumb = sender as Thumb;

        //    if (!(this.AdornedElement is FrameworkElement adornedElement) || hitThumb == null)
        //        return;

        //    FrameworkElement parentElement = adornedElement.Parent as FrameworkElement;

        //    // Ensure that the Width and Height are properly initialized after the resize.
        //    EnforceSize(adornedElement);

        //    // Change the size by the amount the user drags the mouse, as long as it's larger 
        //    // than the width or height of an adorner, respectively.
        //    adornedElement.Height = Math.Max(adornedElement.Height - args.VerticalChange, hitThumb.DesiredSize.Height);
        //}
        //private void HandleMiddleBottom(object sender, DragDeltaEventArgs args)
        //{
        //    Thumb hitThumb = sender as Thumb;

        //    if (!(this.AdornedElement is FrameworkElement adornedElement) || hitThumb == null)
        //        return;

        //    FrameworkElement parentElement = adornedElement.Parent as FrameworkElement;

        //    // Ensure that the Width and Height are properly initialized after the resize.
        //    EnforceSize(adornedElement);

        //    // Change the size by the amount the user drags the mouse, as long as it's larger 
        //    // than the width or height of an adorner, respectively.
        //    adornedElement.Height = Math.Max(args.VerticalChange + adornedElement.Height, hitThumb.DesiredSize.Height);
        //}
        //ends


        // Handler for resizing from the bottom-right.
        private void HandleBottomRight(object sender, DragDeltaEventArgs args)
        {
            Thumb hitThumb = sender as Thumb;

            if (!(this.AdornedElement is FrameworkElement adornedElement) || hitThumb == null)
                return;

            FrameworkElement parentElement = adornedElement.Parent as FrameworkElement;

            // Ensure that the Width and Height are properly initialized after the resize.
            EnforceSize(adornedElement);

            // Change the size by the amount the user drags the mouse, as long as it's larger 
            // than the width or height of an adorner, respectively.
            adornedElement.Width = Math.Max(adornedElement.Width + args.HorizontalChange, hitThumb.DesiredSize.Width);
            adornedElement.Height = Math.Max(args.VerticalChange + adornedElement.Height, hitThumb.DesiredSize.Height);
        }

        // Handler for resizing from the bottom-left.
        private void HandleBottomLeft(object sender, DragDeltaEventArgs args)
        {
            Thumb hitThumb = sender as Thumb;

            if (!(AdornedElement is FrameworkElement adornedElement) || hitThumb == null)
                return;

            FrameworkElement parentElement = adornedElement.Parent as FrameworkElement;

            // Ensure that the Width and Height are properly initialized after the resize.
            EnforceSize(adornedElement);

            // Change the size by the amount the user drags the mouse, as long as it's larger 
            // than the width or height of an adorner, respectively.
            adornedElement.Width = Math.Max(adornedElement.Width - args.HorizontalChange, hitThumb.DesiredSize.Width);
            adornedElement.Height = Math.Max(args.VerticalChange + adornedElement.Height, hitThumb.DesiredSize.Height);
        }

        // Handler for resizing from the top-right.
        private void HandleTopRight(object sender, DragDeltaEventArgs args)
        {
            Thumb hitThumb = sender as Thumb;

            if (!(this.AdornedElement is FrameworkElement adornedElement) || hitThumb == null)
                return;

            FrameworkElement parentElement = adornedElement.Parent as FrameworkElement;

            // Ensure that the Width and Height are properly initialized after the resize.
            EnforceSize(adornedElement);

            // Change the size by the amount the user drags the mouse, as long as it's larger 
            // than the width or height of an adorner, respectively.
            adornedElement.Width = Math.Max(adornedElement.Width + args.HorizontalChange, hitThumb.DesiredSize.Width);
            adornedElement.Height = Math.Max(adornedElement.Height - args.VerticalChange, hitThumb.DesiredSize.Height);
        }

        // Handler for resizing from the top-left.
        private void HandleTopLeft(object sender, DragDeltaEventArgs args)
        {
            Thumb hitThumb = sender as Thumb;

            if (!(AdornedElement is FrameworkElement adornedElement) || hitThumb == null)
                return;

            // Ensure that the Width and Height are properly initialized after the resize.
            EnforceSize(adornedElement);

            // Change the size by the amount the user drags the mouse, as long as it's larger 
            // than the width or height of an adorner, respectively.
            adornedElement.Width = Math.Max(adornedElement.Width - args.HorizontalChange, hitThumb.DesiredSize.Width);
            adornedElement.Height = Math.Max(adornedElement.Height - args.VerticalChange, hitThumb.DesiredSize.Height);
        }

        // Arrange the Adorners.
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (AdornedElement is FrameworkElement frameworkElement)
            {
                // desiredWidth and desiredHeight are the width and height of the element that's being adorned.  
                // These will be used to place the ResizingAdorner at the corners of the adorned element.  
                double desiredWidth = frameworkElement.ActualWidth;
                double desiredHeight = frameworkElement.ActualHeight;

                // adornerWidth & adornerHeight are used for placement as well.
                double adornerWidth = frameworkElement.Width;
                double adornerHeight = frameworkElement.Height;

                if ((adornerWidth > 2000 || adornerHeight > 2000) && frameworkElement is Image image)
                {
                    image.Height = 200;
                    image.Width = 200;
                }

                topRight.Arrange(new Rect(desiredWidth - adornerWidth / 2, -adornerHeight / 2, adornerWidth, adornerHeight));//RNTP
                topLeft.Arrange(new Rect(-adornerWidth / 2, -adornerHeight / 2, adornerWidth, adornerHeight));//RNTP

                bottomLeft.Arrange(new Rect(-adornerWidth / 2, desiredHeight - adornerHeight / 2, adornerWidth, adornerHeight));//RNTP
                bottomRight.Arrange(new Rect(desiredWidth - adornerWidth / 2, desiredHeight - adornerHeight / 2, adornerWidth, adornerHeight));//RNTP

                //MiddleLeft.Arrange(new Rect(-adornerWidth / 2, ((-adornerHeight / 2) + (desiredHeight - adornerHeight / 2)) / 2, adornerWidth, adornerHeight));//RNTP
                //MiddleRight.Arrange(new Rect(desiredWidth - adornerWidth / 2, ((-adornerHeight / 2) + (desiredHeight - adornerHeight / 2)) / 2, adornerWidth, adornerHeight));//RNTP

                //MiddleTop.Arrange(new Rect(((-adornerWidth / 2) + (desiredWidth - adornerWidth / 2)) / 2, -adornerHeight / 2, adornerWidth, adornerHeight));//RNTP
                //MiddleBottom.Arrange(new Rect(((-adornerWidth / 2) + (desiredWidth - adornerWidth / 2)) / 2, desiredHeight - adornerHeight / 2, adornerWidth, adornerHeight));//RNTP
            }

            // Return the final size.
            return finalSize;
        }

        // Helper method to instantiate the corner Thumbs, set the Cursor property, 
        // set some appearance properties, and add the elements to the visual tree.
        void BuildAdornerCorner(ref Thumb cornerThumb, Cursor customizedCursor)
        {
            if (cornerThumb != null)
                return;

            cornerThumb = new Thumb
            {
                Cursor = customizedCursor,
                Height = 10,
                Width = 10,
                Opacity = 0.4,
                Background = new SolidColorBrush(Colors.MediumBlue)
            };

            visualChildren.Add(cornerThumb);
        }

        // This method ensures that the Widths and Heights are initialized.  Sizing to content produces
        // Width and Height values of Double.NaN.  Because this Adorner explicitly resizes, the Width and Height
        // need to be set first.  It also sets the maximum size of the adorned element.
        void EnforceSize(FrameworkElement adornedElement)
        {
            if (adornedElement.Width.Equals(double.NaN))
                adornedElement.Width = adornedElement.DesiredSize.Width;

            if (adornedElement.Height.Equals(double.NaN))
                adornedElement.Height = adornedElement.DesiredSize.Height;

            if (adornedElement.Parent is FrameworkElement parent)
            {
                adornedElement.MaxHeight = parent.ActualHeight;
                adornedElement.MaxWidth = parent.ActualWidth;
            }
        }
        // Override the VisualChildrenCount and GetVisualChild properties to interface with 
        // the adorner's visual collection.
        protected override int VisualChildrenCount => visualChildren.Count;
        protected override Visual GetVisualChild(int index) => visualChildren[index];
    }
}

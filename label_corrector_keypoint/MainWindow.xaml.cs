using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace label_corrector_keypoint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // we have 13 points and 1 bbox
        Ellipse[] kpoints = new Ellipse[13];
        Image imageMain;
        TextBlock[] textBlocks = new TextBlock[13];
        TextBox textBoxX;
        TextBox textBoxY;
        // srote current images path 
        string currentImagePath;
        string fn = "000001188.jpg";
        public MainWindow()
        {
            InitializeComponent();
            this.imageMain = CanvasMain.Children[0] as Image;
            this.textBoxX = this.TextBoxX;
            this.textBoxY = this.TextBoxY;
            this.textBoxX.Text = "0";
            this.textBoxY.Text = "0";
            this.currentImagePath = "C:\\Users\\phamt\\Downloads";
            // set text in front of kpoints[0]
            this.textBlocks[0] = new TextBlock();
            this.textBlocks[0].Text = "1";
            this.textBlocks[0].Foreground = Brushes.Green;
            Canvas.SetTop(this.textBlocks[0], 20);
            Canvas.SetLeft(this.textBlocks[0], 20);
            CanvasMain.Children.Add(this.textBlocks[0]);

            this.kpoints[0] = new Ellipse();
            this.kpoints[0].Fill = Brushes.Blue;
            this.kpoints[0].Width = 10;
            this.kpoints[0].Height = 10;
            Canvas.SetTop(this.kpoints[0], 20);
            Canvas.SetLeft(this.kpoints[0], 20);
            this.kpoints[0].PreviewMouseDown += kpoint_PreviewMouseDown;
            CanvasMain.Children.Add(this.kpoints[0]);

            // pair text with kpoint
            this.textBlocks[0].Tag = this.kpoints[0];
            this.kpoints[0].Tag = this.textBlocks[0];


        }
        UIElement dragObject = null;
        Point offset;
        TextBlock textBlock;

        private void kpoint_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.dragObject = sender as UIElement;
            this.offset = e.GetPosition(this.CanvasMain);
            this.offset.Y -= Canvas.GetTop(this.dragObject);
            this.offset.X -= Canvas.GetLeft(this.dragObject);
            // capture mouse
            this.CanvasMain.CaptureMouse();
        }
        private void CanvasMain_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (this.dragObject == null)
                return;
            var position = e.GetPosition(sender as IInputElement);
            Canvas.SetTop(this.dragObject, position.Y - this.offset.Y);
            Canvas.SetLeft(this.dragObject, position.X - this.offset.X);
            // move tag of kpoint too 
            Ellipse ellipse = this.dragObject as Ellipse;
            this.textBlock = ellipse.Tag as TextBlock;
            Canvas.SetTop(this.textBlock, Canvas.GetTop(this.dragObject));
            Canvas.SetLeft(this.textBlock, Canvas.GetLeft(this.dragObject));
            Point p1 = new Point(Canvas.GetLeft(this.kpoints[0]), Canvas.GetTop(this.kpoints[0]));
            p1.X += this.kpoints[0].Width / 2;
            p1.Y += this.kpoints[0].Height / 2;
            p1.X = p1.X / this.imageMain.ActualWidth * this.imageMain.Source.Width;
            p1.Y = p1.Y / this.imageMain.ActualHeight * this.imageMain.Source.Height;
            this.textBoxX.Text = p1.X.ToString();
            this.textBoxY.Text = p1.Y.ToString();
        }

        private void CanvasMain_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            this.dragObject = null;
            this.CanvasMain.ReleaseMouseCapture();
        }

        private void ButtonReload_Click(object sender, RoutedEventArgs e)
        {
            // join path of currentImagePath and fn
            this.imageMain.Source = new BitmapImage(new Uri(System.IO.Path.Combine(this.currentImagePath, this.fn)));
            // get max resolution of screen and set it to image, scale image to fit 70% of screen
            this.imageMain.Width = this.imageMain.Source.Width;
            this.imageMain.Height = this.imageMain.Source.Height;
            double width = SystemParameters.PrimaryScreenWidth * 0.7;
            double height = SystemParameters.PrimaryScreenHeight * 0.7;
            if (this.imageMain.Width > width)
            {
                this.imageMain.Width = width;
                this.imageMain.Height = (int)width * this.imageMain.Source.Height / this.imageMain.Source.Width;
            }
            if (this.imageMain.Height > height)
            {
                this.imageMain.Height = height;
                this.imageMain.Width = height * this.imageMain.Source.Width / this.imageMain.Source.Height;
                // round it 
                this.imageMain.Width = (int)this.imageMain.Width;
            }


            // scale canvas to fit image
            this.CanvasMain.Width = this.imageMain.Width + 20;
            this.CanvasMain.Height = this.imageMain.Height + 50;
            // scale main window to fit image
            this.Width = this.CanvasMain.Width * 4 / 3;
            this.Height = this.CanvasMain.Height;


        }

        private void ButtonBrowse_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonPrev_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            // TODO: now i have to do is normalize this with Image. 
            // Get location of kpoints[0] and kpoint2 on Image children of CanvasMain
            // then normalize it with Image
            // then save it to file
            Point p1 = new Point(Canvas.GetLeft(this.kpoints[0]), Canvas.GetTop(this.kpoints[0]));
            p1.X += this.kpoints[0].Width / 2;
            p1.Y += this.kpoints[0].Height / 2;
            // normalize
            p1.X = p1.X / this.imageMain.ActualWidth * this.imageMain.Source.Width;
            p1.Y = p1.Y / this.imageMain.ActualHeight * this.imageMain.Source.Height;
            Console.WriteLine("p1: " + p1.ToString());

        }
    }


}

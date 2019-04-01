using System.Drawing;
using System.Windows.Forms;


namespace Coursework_Project {
    public partial class frmViewMusic : Form {
        const string dbConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='C:\Users\Thomas\source\repos\A-level Coursework\Desktop Programs\Coursework Project\CourseworkDatabase.mdf';Integrated Security=True;Connect Timeout=30";

        midiFile loadedMidiFile;

        public frmViewMusic(midiFile inputFile) {
            string errorString;

            loadedMidiFile = inputFile;

            InitializeComponent();

            //Load bitmaps
            loadedMidiFile.GenerateMusicPage(picDisplay.Width, picDisplay.Height, 0, false, false, out Bitmap pageToDisplay,out int nextLine, out errorString);
            picDisplay.Image = pageToDisplay;
        }


    }
}

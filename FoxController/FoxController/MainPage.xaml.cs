using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FoxController
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MediaPlayer player;
        private struct Tone
        {
            private string name;
            private string path;

            public Tone(string newName, string newPath)
            {
                name = newName;
                path = newPath;
            }

            public string getName()
            {
                return this.name;
            }

            public string getPath()
            {
                return this.path;
            }
        }

        public MainPage()
        {
            this.InitializeComponent();
            this.initializeGpio();
        }

        private void initializeGpio()
        {

        }

        private void respondToIdSwitch()
        {

        }

        private void respondToStartStopSwitch()
        {

        }

        private void playId()
        {

        }

        private void playTones(int numTones)
        {

        }

        private async void playTone(Tone tone)
        {
            StorageFile file = await StorageFile.GetFileFromPathAsync(tone.getPath());
            player.SetFileSource(file);
            player.Play();
        }
        

    }
}

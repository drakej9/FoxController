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
using Windows.Media.SpeechSynthesis;
using Windows.Devices.Gpio;
using Windows.UI.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FoxController
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MediaElement mediaElement = new MediaElement();
        private const int BUTTON_PIN = 4;
        private GpioPin buttonPin;
        SpeechSynthesisStream idStream;
        private string id = "This is the C A L L S I G N fox transmitter.";
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
            this.initializeId();
        }

        private void initializeGpio()
        {
            var gpio = GpioController.GetDefault();
            // Show an error if there is no GPIO controller

            if (gpio == null)
            {
                return;
            }

            buttonPin = gpio.OpenPin(BUTTON_PIN);

            // Check if input pull-up resistors are supported
            if (buttonPin.IsDriveModeSupported(GpioPinDriveMode.InputPullUp))
                buttonPin.SetDriveMode(GpioPinDriveMode.InputPullUp);
            else
                buttonPin.SetDriveMode(GpioPinDriveMode.Input);

            // Set a debounce timeout to filter out switch bounce noise from a button press
            buttonPin.DebounceTimeout = TimeSpan.FromMilliseconds(50);

            buttonPin.ValueChanged += respondToIdSwitch;

        }

        private async void initializeId()
        {
            var synth = new SpeechSynthesizer();
            idStream = await synth.SynthesizeTextToStreamAsync(id);
        }

        private void respondToIdSwitch(GpioPin sender, GpioPinValueChangedEventArgs e)
        {
            var task = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                playId();
            });
        }

        private void respondToStartStopSwitch()
        {

        }

        private void playId()
        {
            mediaElement.SetSource(idStream, idStream.ContentType);
            mediaElement.Play();
        }

        private void playTones(int numTones)
        {

        }

        //private async void playTone(Tone tone)
        //{
        //    StorageFile file = await StorageFile.GetFileFromPathAsync(tone.getPath());
        //    mediaElement.SetFileSource(file);
        //    mediaElement.Play();
        //}
        

    }
}

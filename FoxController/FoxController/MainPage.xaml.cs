using System;
//using System.IO;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.Media.Core;
using Windows.Storage;
using Windows.Media.SpeechSynthesis;
using Windows.Devices.Gpio;
using Windows.UI.Core;
using Windows.System.Threading;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FoxController
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MediaElement mediaElement = new MediaElement();
        //private const int ID_PIN = 4;
        private const int START_SEQUENCE_PIN = 5;
        private bool sequenceStarted = true;
        private TimeSpan period = TimeSpan.FromSeconds(20);
        private ThreadPoolTimer periodicTimer;
        
        SpeechSynthesisStream idStream;
        private string id = "This is the C A L L S I G N fox transmitter; part of the Boy Scout Troop 1 11 radio direction finding exercise.";

        public MainPage()
        {
            this.InitializeComponent();
            this.initializeGpio();
            this.initializeAudio();
            this.initializeTimer();
        }

        private void initializeGpio()
        {
            var gpio = GpioController.GetDefault();
            // Show an error if there is no GPIO controller

            if (gpio == null)
            {
                return;
            }

            //GpioPin idPin = gpio.OpenPin(ID_PIN);
            GpioPin startSequencePin = gpio.OpenPin(START_SEQUENCE_PIN);

            // check if input pull-up resisitors are supprted
            //checkAndSetPullUp(idPin);
            checkAndSetPullUp(startSequencePin);

            // Set a debounce timeout to filter out switch bounce noise from a button press
            //idPin.DebounceTimeout = TimeSpan.FromMilliseconds(50);
            startSequencePin.DebounceTimeout = TimeSpan.FromMilliseconds(50);

            //idPin.ValueChanged += respondToIdSwitch;
            startSequencePin.ValueChanged += respondToStartSequenceSwitch;
        }

        private void checkAndSetPullUp(GpioPin pin)
        {
            // Check if input pull-up resistors are supported
            if (pin.IsDriveModeSupported(GpioPinDriveMode.InputPullUp))
                pin.SetDriveMode(GpioPinDriveMode.InputPullUp);
            else
                pin.SetDriveMode(GpioPinDriveMode.Input);
        }

        private async void initializeAudio()
        {
            var synth = new SpeechSynthesizer();
            idStream = await synth.SynthesizeTextToStreamAsync(id);
        }

        private void initializeTimer()
        {
            periodicTimer = ThreadPoolTimer.CreatePeriodicTimer(async (CoreComponentInputSource) =>
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.High,
                () =>
                {
                    if (sequenceStarted == true)
                    {
                        //playId();
                        mediaElement.SetSource(idStream, idStream.ContentType);
                        mediaElement.Position = new TimeSpan(0, 0, 0);
                        mediaElement.Play();
                    }
                });
            }, period);
        }

        //private void respondToIdSwitch(GpioPin sender, GpioPinValueChangedEventArgs e)
        //{
            
        //    var task = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
        //    {
        //        playId();
        //    });
        //}

        private void respondToStartSequenceSwitch(GpioPin sender, GpioPinValueChangedEventArgs e)
        {
            if (sequenceStarted == false)
            {
                sequenceStarted = true;
            }
            else
            {
                sequenceStarted = false;
            }
        }

        private void playId()
        {
            mediaElement.SetSource(idStream, idStream.ContentType);
            mediaElement.Position = new TimeSpan(0,0,0);
            mediaElement.Play();
        }
    }
}

﻿using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Transceivers;
using Meadow.Hardware;
using System;
using System.Text;
using System.Threading;

namespace Transceivers.Nrf24l01_Sample
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        RgbLed led;
        Nrf24l01 radio;
        string address = "00001";

        public MeadowApp()
        {
            led = new RgbLed(Device, Device.Pins.OnboardLedRed, Device.Pins.OnboardLedGreen, Device.Pins.OnboardLedBlue);
            led.SetColor(RgbLed.Colors.Red);

            var config = new SpiClockConfiguration(10000000, SpiClockConfiguration.Mode.Mode0);
            ISpiBus spiBus = Device.CreateSpiBus(Device.Pins.SCK, Device.Pins.MOSI, Device.Pins.MISO, config);

            radio = new Nrf24l01(
                    device: Device,
                    spiBus: spiBus,
                    chipEnablePin: Device.Pins.D13,
                    chipSelectLine: Device.Pins.D12,
                    interruptPin: Device.Pins.D00);

            radio.SetChannel(76);
            radio.OpenReadingPipe(0, Encoding.UTF8.GetBytes(address));
            radio.SetPALevel(0);
            radio.StartListening();

            led.SetColor(RgbLed.Colors.Green);

            byte[] text = new byte[32];

            while (true)
            {
                // TRANSMITER CODE
                //string helloWorld = "Hello World";
                //radio.Write(Encoding.Unicode.GetBytes(helloWorld), (byte)(helloWorld.Length));
                //Console.WriteLine($"Sending: {helloWorld} \n");
                //Thread.Sleep(1000);

                // RECEIVER CODE
                if (radio.IsAvailable())
                {
                    //Console.WriteLine("Yo!");
                    radio.Read(text, (byte) text.Length);
                }
                Console.WriteLine();

                Thread.Sleep(10000);
            }           
        }
    }
}
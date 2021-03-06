﻿using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Hardware;

namespace Leds.APA102_Sample
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        Apa102 apa102;

        public MeadowApp()
        {
            Initialize();

            Pulse(Color.Salmon);
            RunColors();
        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");
            ISpiBus spiBus = Device.CreateSpiBus();

            //Not used but is need to create the SPI Peripheral
            IDigitalOutputPort spiPeriphChipSelect = Device.CreateDigitalOutputPort(Device.Pins.D04);
            apa102 = new Apa102(spiBus, spiPeriphChipSelect, 150, Apa102.PixelOrder.BGR);
        }

        void Pulse(Color color)
        {
            float brightness = 0.05f;
            bool forward = true;

            while (true)
            {
                for(uint i = 0; i < apa102.NumberOfLeds; i++)
                {
                    apa102.SetLed(i, color, brightness);
                }

                if(forward) { brightness += 0.05f; }
                else { brightness -= 0.05f; }

                if(brightness <= 0.05f)
                {
                    forward = true;
                }
                if(brightness >= 0.6f)
                {
                    forward = false;
                }

                apa102.Show();
            }
        }



        void RunColors()
        {
            uint last = 9999;

            bool forward = true;
            uint index = 0;

            while(true)
            {
                if (last != 9999) { apa102.SetLed(last, Color.Black); }
                apa102.SetLed(index, Color.Turquoise);
                last = index;

                if(forward) { index++; }
                else { index--; }

                apa102.Show();

                if(index == apa102.NumberOfLeds - 1)
                {
                    forward = false;
                }
                if(index == 0)
                {
                    forward = true;
                }
            }

            for (uint i = 0; i < apa102.NumberOfLeds; i++)
            {
                if(last != 9999) { apa102.SetLed(last, Color.Black); }
                apa102.SetLed(i, Color.Turquoise);
                last = i;
                apa102.Show();
            }
        }

        void Run()
        {
            Console.WriteLine("Run...");
            apa102.Clear();
            apa102.Show();

            Thread.Sleep(2000);
            apa102.SetLed(0, Color.Red, 0.5f);
            apa102.SetLed(1, Color.White);
            apa102.SetLed(2, Color.Blue);

            Thread.Sleep(2000);
            apa102.Show();

            Thread.Sleep(2000);
            apa102.AutoWrite = true;
            apa102.SetLed(0, Color.Green);
            apa102.SetLed(1, Color.Yellow);
            apa102.SetLed(2, Color.Pink);

            Thread.Sleep(5000);
            apa102.Clear();

        }
    }
}
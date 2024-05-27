using System;
using System.Device.Spi;
using System.Drawing;
using Iot.Device.Graphics;
using Iot.Device.Ws28xx;
using System.Text.Json;

// using GPIO 10

// Yellow lazers
// Light player color when starting lazer
// from p1 to p2
// explosion in color of target player
// animate moving
// animate board effects

// 18 leds per player/seat

namespace MRR_CLG
{
    public class LEDs
    {
        private const int LEDcount = 144;
        private Ws2812b device;

        private SpiConnectionSettings settings;
        private SpiDevice spi;
        private RawPixelContainer image;

        public LEDs()
        {
            
            settings = new SpiConnectionSettings(0, 0) {
                ClockFrequency = 2_400_000,
                Mode = SpiMode.Mode0,
                DataBitLength = 8
            };
            
            spi = SpiDevice.Create(settings);
            device = new Ws2812b(spi, LEDcount);
            image = device.Image;

            Animation(2);

        }

        public string Animation(int animate)
        {
            //BitmapImage image = device.Image;
            image.Clear();
            for(int i=0; i<LEDcount; i++)
            {
                image.SetPixel(i, 0, Color.Black);
            }

            //string strJson = JsonSerializer.Serialize<Ws2812b>(device);
            Console.WriteLine("LED Animate:{0}", animate);            

            switch(animate)
            {
                case 0:
                    for(int i=0; i<LEDcount; i++)
                    {
                        image.SetPixel(i, 0, Color.Black);
                    }
                    image.SetPixel(0,0,Color.Blue);
                    device.Update();
                    break;
                case 1:
                    for(int i=0; i<LEDcount; i++)
                    {
                        image.SetPixel(i, 0, Color.Blue);
                        device.Update();
                        System.Threading.Thread.Sleep(10);
                    }
                    for(int i=0; i<LEDcount; i++)
                    {
                        image.SetPixel(i, 0, Color.Black);
                        //Console.Write(i);
                        //Console.Write("-");
                        device.Update();
                        System.Threading.Thread.Sleep(10);
                    }
                    //Console.WriteLine("end");
                    break;
                case 2:
                    for(int i=0; i<LEDcount+10; i++)
                    {
                        if (i<LEDcount)
                        {
                            image.SetPixel(i, 0, Color.Yellow);
                        }
                        if (i>=10)
                        {
                            image.SetPixel(i-10, 0, Color.Black);
                        }

                        //Console.Write(i);
                        //Console.Write("-");
                        device.Update();
                        System.Threading.Thread.Sleep(1);
                    }
                    //Console.WriteLine("end");
                    break;
                case 3:
                    device.Update();
                    break;
            }
            return "animation " + animate.ToString();
        }
    }
}


namespace hello_ws2812
{
    class Program1
    {
        private const int count = 30; 
        static void Main1()
        {
            var settings = new SpiConnectionSettings(0, 0) {
                ClockFrequency = 2_400_000,
                Mode = SpiMode.Mode0,
                DataBitLength = 8
                //ChipSelectLine = 18
            };
            Console.WriteLine("Starting");
            Console.WriteLine(settings.ToString());
            
            //var spi = new UnixSpiDevice(settings);
            var spi = SpiDevice.Create(settings);
            var device = new Ws2812b(spi, count);
            Console.WriteLine("create ws");

            // Display basic colors for 5 sec
            RawPixelContainer image = device.Image;
            image.Clear();
            image.SetPixel(0, 0, Color.Orange);
            image.SetPixel(1, 0, Color.Red);
            image.SetPixel(2, 0, Color.Green);
            image.SetPixel(3, 0, Color.Blue);
            image.SetPixel(4, 0, Color.Yellow);
            image.SetPixel(5, 0, Color.Cyan);
            image.SetPixel(6, 0, Color.Magenta);
            image.SetPixel(7, 0, Color.FromArgb(unchecked((int)0xffff8000)));
            device.Update();
            Console.WriteLine("update1");
            System.Threading.Thread.Sleep(5000);
    
            // Chase some blue leds
            for(int i=0; i<10; i++)
            {
                Console.Write("image");
                Console.WriteLine(i.ToString());
                image.Clear();
                for(int j=0; j<count; j++)
                {
                    image.SetPixel(j, 0, Color.LightBlue);
                    device.Update();
                    System.Threading.Thread.Sleep(10);
                    image.SetPixel(j, 0, Color.Blue);
                    device.Update();
                    System.Threading.Thread.Sleep(25);
                }
            }

            // Color Fade
            int r=255;
            int g=0;
            int b=0;
                Console.WriteLine("fade");

            while (! Console.KeyAvailable) {
                if(r > 0 && b == 0){
                    r--;
                    g++;
                }
                if(g > 0 && r == 0){
                    g--;
                    b++;
                }
                if(b > 0 && g == 0){
                    r++;
                    b--;
                }

                image.Clear(Color.FromArgb(r,g,b));
                device.Update();
                System.Threading.Thread.Sleep(10);
            }    


            image.Clear();
            device.Update();

            Console.WriteLine("Hello Pi!");
        }
    }
}
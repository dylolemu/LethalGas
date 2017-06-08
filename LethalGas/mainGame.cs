﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LethalGas
{
    public partial class mainGame : UserControl
    {
        public mainGame()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }
        Point[] triangle = new Point[12];

        List<Image> characters = new List<Image>();
        List<Image> charactersL = new List<Image>();
        List<Pedestrian> peds = new List<Pedestrian>();
        Random randNum = new Random();
        Rectangle pic1 = new Rectangle(0, 0, 10, 10);

        int pedSpawnTimer;
        int img, imgStill;
        int fartTimer;
        bool still, fart, pause;
        int brightness, day;
        bool right, left;
        int position;

        private void mainGame_Load(object sender, EventArgs e)
        {
            //Pedestrian ped1 = new Pedestrian(this.Width, -1, 3, "dink", charactersL, pic1);
            //peds.Add(ped1);
            Form1.mainGameMusic.Play();
            characters.Add(Properties.Resources.dylonStillRN);
            characters.Add(Properties.Resources.dylonWalk1RN);
            characters.Add(Properties.Resources.dylonWalk2R);
            characters.Add(Properties.Resources.dylonIdle1);
            characters.Add(Properties.Resources.dylonIdle2);
            characters.Add(Properties.Resources.dylonFart);
            charactersL.Add(Properties.Resources.dylonStillN);
            charactersL.Add(Properties.Resources.dylonWalk1N);
            charactersL.Add(Properties.Resources.dylonWalk2);
            lightPoints();
        }

        public void lightPoints()
        {
            triangle[0] = new Point(0, 0);
            triangle[1] = new Point(0, this.Height-150);
            triangle[2] = new Point(this.Width - 226, this.Height - 445);
            triangle[3] = new Point(this.Width - 145, this.Height - 445);
            triangle[4] = new Point(this.Width, this.Height - 320);
            triangle[5] = new Point(this.Width, this.Height);
            triangle[6] = new Point(this.Width - 111, this.Height);
            triangle[7] = new Point(this.Width - 151, this.Height - 385);
            triangle[8] = new Point(this.Width - 221, this.Height - 385);
            triangle[9] = new Point(this.Width - 261, this.Height);
            triangle[10] = new Point(this.Width, this.Height);
            triangle[11] = new Point(this.Width, 0);
        }

        private void mainGame_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            imgStill = 0;
            if (e.KeyCode == Keys.Escape) { pause = true; }
            if (!pause)
            {
                if (e.KeyCode == Keys.Right && !fart ) { right = true; }
                if (e.KeyCode == Keys.Left && !fart ) { left = true; }
                if (e.KeyCode == Keys.Space)
                { right = false; left = false; fart = true; }
            }
            if (pause != true)
            {
                if (e.KeyCode == Keys.Right && fart == false) { right = true; }
                if (e.KeyCode == Keys.Left && fart == false) { left = true; }
                if (e.KeyCode == Keys.Space) { right = false; left = false; fart = true; }
            }
            else
            {
                if (e.KeyCode == Keys.Space) { pause = false; }
                if(e.KeyCode == Keys.M)
                {
                    // Create an instance of the SecondScreen
                    MainScreen cs = new MainScreen();
                    cs.Location = new Point(this.Left, this.Top);
                    // Add the User Control to the Form
                    Form f = this.FindForm();
                    f.Controls.Remove(this);
                    f.Controls.Add(cs);
                }
            }
            if (brightness < 245)
            {
                if (e.KeyCode == Keys.B) { brightness += 10; }
            }
            if(brightness > 12 )
            { 
                if (e.KeyCode == Keys.M) { brightness -= 10; }
            }

        }
        private void mainGame_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right) { right = false; }
            if (e.KeyCode == Keys.Left) { left = false; }
            if (e.KeyCode == Keys.Space) { fart = false; }
        }

        public void dayChange()
        {
            day++;
            if (day < 250) { brightness++; }
            else if (day >= 800 && day < 949) { brightness--; }
            else if (day == 1600) { day = 0; }
            backBrush.Color = Color.FromArgb(brightness, 0, 0, 0);
        }

        public void fartMeter()
        {
            //fart Meter
            blockBrush.Color = Color.FromArgb(200 - fartTimer / 4, 50, 128, 40);
            blockBrush2.Color = Color.FromArgb(200 - fartTimer / 3, 0, 100, 0);
            blockBrush3.Color = Color.FromArgb(fartTimer / 2, 165, 42, 42);

            if (fart && fartTimer > 0)
            {
                fartTimer--;
            }
            else if (fartTimer < 260) { fartTimer++; }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            dayChange();
            fartMeter();

            #region Pedestrians

            pedSpawnTimer++;

            SpawnNPC();

            foreach(Pedestrian p in peds)
            {
                p.Move();
                //label1.Text = ;
            }

            #endregion
            if (right && position < this.Width - 119)
            {
                left = false;
                position += 5;
                still = false;
                imageChange();
            }
            else if (left && position > -40)
            {
                right = false;
                still = false;
                position -= 5;
                imageChange();
            }
            else
            {
                still = true;
                imgStill++;
                if (imgStill == 60)
                {
                    imgStill = 0;
                }
            }
            Refresh();
        }

        public void imageChange()
        {
            img++;
            if (img == 40)
            {
                img = 0;
            }
        }

        SolidBrush blockBrush = new SolidBrush(Color.Green);
        SolidBrush blockBrush2 = new SolidBrush(Color.Green);
        SolidBrush blockBrush3 = new SolidBrush(Color.Green);
        SolidBrush backBrush = new SolidBrush(Color.FromArgb(190,0,0,0));
        SolidBrush backBrush1 = new SolidBrush(Color.FromArgb(190, 0, 0, 0));
        SolidBrush backBrush2 = new SolidBrush(Color.FromArgb(190, 0, 0, 0));

        private void mainGame_Paint(object sender, PaintEventArgs e)
        {
            #region ped images

            foreach(Pedestrian p in peds)
            {
                if (p.direction == 1)
                {
                    if (p.imgDex < 10)
                    {
                        e.Graphics.DrawImage(p.Images[0], p.pic);
                    }
                    if (p.imgDex < 20 && p.imgDex >= 10)
                    {
                        e.Graphics.DrawImage(p.Images[1], p.pic);
                    }
                    if (p.imgDex >= 20 && p.imgDex < 30)
                    {
                        e.Graphics.DrawImage(p.Images[0], p.pic);
                    }
                    if (p.imgDex >= 30 && p.imgDex <= 40)
                    {
                        e.Graphics.DrawImage(p.Images[2], p.pic);
                    }
                }
                else if (p.direction == -1)
                {
                    if (p.imgDex < 10)
                    {
                        e.Graphics.DrawImage(p.Images[0], p.pic);
                    }
                    if (p.imgDex < 20 && p.imgDex >= 10)
                    {
                        e.Graphics.DrawImage(p.Images[1], p.pic);
                    }
                    if (p.imgDex >= 20 && p.imgDex < 30)
                    {
                        e.Graphics.DrawImage(p.Images[0], p.pic);
                    }
                    if (p.imgDex >= 30 && p.imgDex <= 40)
                    {
                        e.Graphics.DrawImage(p.Images[2], p.pic);
                    }
                }
            }

            #endregion

            if (!still && right)
            {
                if (img < 10)
                {
                    e.Graphics.DrawImage(characters[0], new Rectangle(position, this.Height-329, 150, 300));
                }
                if (img < 20 && img >= 10)
                {
                    e.Graphics.DrawImage(characters[1], new Rectangle(position, this.Height -329, 150, 300));
                }
                if (img >= 20 && img < 30)
                {
                    e.Graphics.DrawImage(characters[0], new Rectangle(position, this.Height - 329, 150, 300));
                }
                if (img >= 30 && img <= 40)
                {
                    e.Graphics.DrawImage(characters[2], new Rectangle(position, this.Height - 329, 150, 300));
                }
            }
            else if (!still && left)
            {
                if (img < 10)
                {
                    e.Graphics.DrawImage(charactersL[0], new Rectangle(position, this.Height - 329, 150, 300));
                }
                if (img < 20 && img >= 10)
                {
                    e.Graphics.DrawImage(charactersL[1], new Rectangle(position, this.Height - 329, 150, 300));
                }
                if (img >= 20 && img < 30)
                {
                    e.Graphics.DrawImage(charactersL[0], new Rectangle(position, this.Height - 329, 150, 300));
                }
                if (img >= 30 && img <= 40)
                {
                    e.Graphics.DrawImage(charactersL[2], new Rectangle(position, this.Height - 329, 150, 300));
                }
            }
            else if (fart) { e.Graphics.DrawImage(characters[5], new Rectangle(position, this.Height-329, 150, 300)); }
          //if player is not moving
            else
            {
                if (imgStill < 30)
                {
                    e.Graphics.DrawImage(characters[3], new Rectangle(position, this.Height - 329, 150, 300));
                }
                if (imgStill <= 60 && imgStill >= 30)
                {
                    e.Graphics.DrawImage(characters[4], new Rectangle(position, this.Height - 329, 150, 300));
                }
            }

            //pause Screen appears
            if(pause)
            {
                e.Graphics.FillRectangle(backBrush1, 0, 0, 1020, 650);
                e.Graphics.DrawImage(Properties.Resources.Pause1, new Rectangle(0,0,1020,650));
            }

            //light
            e.Graphics.FillPolygon(backBrush, triangle);

            //FartMeter
            e.Graphics.FillRectangle(blockBrush, (this.Width / 2) - fartTimer, 50, fartTimer*2, 30);
            e.Graphics.FillRectangle(blockBrush2, (this.Width / 2) - fartTimer, 50, fartTimer * 2, 30);
            e.Graphics.FillRectangle(blockBrush3, (this.Width / 2) - fartTimer, 50, fartTimer * 2, 30);
        }

        public void SpawnNPC()
        {
            if (randNum.Next(0, 100) == 1)
            {
                if (randNum.Next(1,3) == 1)
                {
                    if (randNum.Next(1, 3) == 1)
                    {
                        Pedestrian ped = new Pedestrian(-100, 1, 2, "paul", characters, pic1);
                        peds.Add(ped);
                    }
                    else
                    {
                        Pedestrian ped = new Pedestrian(-100, 1, 3, "paul", characters, pic1);
                        peds.Add(ped);

                    }
                }

                else
                {
                    if (randNum.Next(1, 3) == 1)
                    {
                        Pedestrian ped = new Pedestrian(this.Width, -1, 3, "phil", charactersL, pic1);
                        peds.Add(ped);
                    }
                    else
                    {
                        Pedestrian ped = new Pedestrian(this.Width, -1, 2, "phil", charactersL, pic1);
                        peds.Add(ped);
                    }
                }
            }
        }
    }
}

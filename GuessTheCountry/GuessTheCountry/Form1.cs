using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuessTheCountry
{
    public partial class MainGame : Form
    {
        public MainGame()
        {
            InitializeComponent();
        }

        Dictionary<string, int> ht = new Dictionary<string, int>();
       public struct Country
        {
            public int id;
            public string name;
            public float longit;
            public float latid;
            public int pop;
            public string cont;
        }

        Graphics g;
        Pen pen;

        bool[] verif = new bool[200];

        Country[] countries = new Country[200];

        int guess = -1;

        int currentGuess = -1;

        int gameStarted = 0;

        int totalGuess = 0;

        public  void init()
        {
            if (gameStarted == 1) return;
            for (int i = 1; i <= 169; i++) verif[i] = false;
            Random gen = new Random();
            guess = gen.Next(1, 169);

            btn_make.Enabled = true;
            btnStartSurrender.Text = "Surrender";
            ht.Clear();
            totalGuess = 0;
            BestScore.Text = "Your score is: Unknown";
            //MessageBox.Show(Directory.GetCurrentDirectory());
            string path = Directory.GetCurrentDirectory();
            path = path += "/TextFile1.txt";
            var p = @path;
           // var filePath = @"C:/Users/User/source/repos/GuessTheCountry/GuessTheCountry/Resources/TextFile1.txt";
            //var filePath = Path.Combine(Directory.GetCurrentDirectory(), "\\TextFile1.txt");
            
            using (var streamReader = new StreamReader(p))
            {
                string line;
                while (!string.IsNullOrWhiteSpace(line = streamReader.ReadLine()))
                {
                   
                    int i = 0;
                    int nr = 0;
                    
                    string temp = "";
                        while(line[i] != ',')
                        {
                        nr = nr * 10 + line[i] - 48; i++;
                        }
                    i++;
                    countries[nr].id = nr;
                    while(line[i] != ',')
                    {
                        temp = temp + line[i];
                        i++;
                    }
                    countries[nr].name = temp;
                    temp = "";
                    i++;
                    while (line[i] != ',')
                    {
                        if (line[i] != '.')
                            temp = temp + line[i];
                        else temp = temp + ',';
                        i++;
                    }
                    countries[nr].latid = float.Parse(temp);
                    i++;
                    temp = "";
                    while (line[i] != ',')
                    {
                        if (line[i] != '.')
                            temp = temp + line[i];
                        else temp = temp + ',';
                        i++;
                    }
                    countries[nr].longit = float.Parse(temp);
                    i++;
                    temp = "";
                    while (line[i] != ',')
                    {
                        temp = temp + line[i];
                        i++;
                    }
                    countries[nr].cont = temp;
                    i++;
                    temp = "";
                    while (line[i] != ',')
                    {
                        
                            temp = temp + line[i];
                       
                        i++;
                    }
                    countries[nr].pop = int.Parse(temp);
                    ht.Add(countries[nr].name, countries[nr].id);
                    listBox1.Items.Add(countries[nr].name);
                    temp = "";
                    
                }
              
            }
            refresh();
            gameStarted = 1;
        }

        public void refresh()
        {
            listBox1.Items.Clear();
            
            for(int i= 1; i <= 169; i++)
            {
                if(!verif[i])
                listBox1.Items.Add(countries[i].name);
            }
            if (gameStarted == 0 && listBox1.SelectedIndex != 0) return;
            g = this.CreateGraphics();
            g.Clear(Color.White);
            //pen = new Pen(new SolidBrush(Color.Green));
            SolidBrush brush;

            if (countries[currentGuess].pop == countries[guess].pop) brush = new SolidBrush(Color.Green);
            else if(Math.Abs(countries[currentGuess].pop - countries[guess].pop) <= 50000) brush = new SolidBrush(Color.Yellow);
            else if(Math.Abs(countries[currentGuess].pop - countries[guess].pop) <= 500000) brush = new SolidBrush(Color.Orange);
            else brush = new SolidBrush(Color.Red);

            g.FillEllipse(brush,220,290,65,65);

            PointF[] pointFs = new PointF[3];
            if (countries[currentGuess].pop > countries[guess].pop)
            {
                pointFs[0] = new Point(232, 315);
                pointFs[1] = new Point(272, 315);
                pointFs[2] = new Point(252, 345);
            }
            else
            {
                pointFs[0] = new Point(232, 335);
                pointFs[1] = new Point(272, 335);
                pointFs[2] = new Point(252, 305);
            }

            if(countries[currentGuess].pop != countries[guess].pop)
            g.FillPolygon(new SolidBrush(Color.AntiqueWhite), pointFs);
            label6.Text = countries[currentGuess].pop.ToString();
            
            if(countries[currentGuess].cont == countries[guess].cont)
                g.FillEllipse(new SolidBrush(Color.Green), 340, 290, 65, 65);
            else
                g.FillEllipse(new SolidBrush(Color.Red), 340, 290, 65, 65);

            label9.Text = countries[currentGuess].cont;
            

            if(countries[currentGuess].longit == countries[guess].longit)
            {
                g.FillEllipse(new SolidBrush(Color.Green), 460, 290, 65, 65);
            }
            else if (Math.Abs(countries[currentGuess].longit-countries[guess].longit) <= 10) g.FillEllipse(new SolidBrush(Color.Yellow), 460, 290, 65, 65);
            else if (Math.Abs(countries[currentGuess].longit - countries[guess].longit) <= 35) g.FillEllipse(new SolidBrush(Color.Orange), 460, 290, 65, 65);
            else g.FillEllipse(new SolidBrush(Color.Red), 460, 290, 65, 65);

            if(countries[currentGuess].longit > countries[guess].longit)
            {
                pointFs[0] = new Point(472, 325);
                pointFs[1] = new Point(502, 310);
                pointFs[2] = new Point(502, 340);
            }
            else if(countries[currentGuess].longit < countries[guess].longit)
            {
                pointFs[0] = new Point(512, 325);
                pointFs[1] = new Point(482, 310);
                pointFs[2] = new Point(482, 340);
            }
            if(currentGuess != guess)
            g.FillPolygon(new SolidBrush(Color.AntiqueWhite), pointFs);
            label7.Text = countries[currentGuess].longit.ToString(); 


            if(countries[currentGuess].latid == countries[guess].latid)
                g.FillEllipse(new SolidBrush(Color.Green), 580, 290, 65, 65);
            else if(Math.Abs(countries[currentGuess].latid - countries[guess].latid) <= 10) g.FillEllipse(new SolidBrush(Color.Yellow), 580, 290, 65, 65);
            else if (Math.Abs(countries[currentGuess].latid - countries[guess].latid) <= 35) g.FillEllipse(new SolidBrush(Color.Orange), 580, 290, 65, 65);
            else  g.FillEllipse(new SolidBrush(Color.Red), 580, 290, 65, 65);

            if (countries[currentGuess].latid > countries[guess].latid)
            {
                pointFs[0] = new Point(592, 315);
                pointFs[1] = new Point(632, 315);
                pointFs[2] = new Point(612, 345);
            }
            else if (countries[currentGuess].latid < countries[guess].latid)
            {
                pointFs[0] = new Point(592, 335);
                pointFs[1] = new Point(632, 335);
                pointFs[2] = new Point(612, 305);
            }

            if (currentGuess != guess)
                g.FillPolygon(new SolidBrush(Color.AntiqueWhite), pointFs);

            label8.Text = countries[currentGuess].latid.ToString();

            label10.Text = countries[currentGuess].name;
        }

        public void checkGuess(int id)
        {
            if (id == guess) winner();
            else if (totalGuess >= 15) loser();
            else refresh();
        }

        public void winner()
        {
            btn_make.Enabled = false;
            btnStartSurrender.Text = "New Game";
            pictureBox1.Visible = false;
            label1.Text = "You Won!";
            label1.Visible = true;
            refresh();
        }

        public void loser()
        {

            label1.Text = "You Lose!";
            btn_make.Enabled = false;
            pictureBox1.Visible = false;
            label1.Visible = true;
            if(totalGuess != 0)
            refresh();
            btnStartSurrender.Text = "New Game";
        }

        private void btnStartSurrender_Click(object sender, EventArgs e)
        {
            if(btnStartSurrender.Text == "Start")
            init();
            else if(btnStartSurrender.Text == "Surrender")
            {
                loser();
            }
            else
            {
                gameStarted = 0;
                btnStartSurrender.Text = "Start";
                label1.Visible = false;
                pictureBox1.Visible = true;
                btn_make.Enabled = false;
                if(totalGuess!=0)
                g.Clear(Color.White);
                label6.Text = "";
                label7.Text = "";
                label8.Text = "";
                label9.Text = "";
                label10.Text = "Current Country:";
            }
        }

        private void btn_make_Click(object sender, EventArgs e)
        {
            totalGuess++;
            BestScore.Text = "Your score is: " + totalGuess.ToString();
            currentGuess = ht[listBox1.SelectedItem.ToString()];
            verif[currentGuess] = true;
            checkGuess(currentGuess);
        }

        private void btnRules_Click(object sender, EventArgs e)
        {
            if(gameStarted == 1)
            MessageBox.Show(countries[guess].name);
        }
    }
}

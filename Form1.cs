using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SRTmodifier
{

    public partial class Form1 : Form
    {
        //public string path= @"C:\Users\Guillermo Duarte\Downloads\Whiplash.2014.720p.WEB-DL.AAC2.0.H264-RARBG.srt";
        private double sum = 0;
        private double finesse = 1;
        private bool mod = false;
        List<Subtitle> Subtitles;
        private List<Control> _subControls = new List<Control>();
        private List<Control> _labels = new List<Control>();

        private long finesseToTicks
        {
            get
            {
                return (long)(finesse * 10000000);
            }
        }
        public Form1()
        {
            InitializeComponent();
            this.Text = "SRT Modifier";
            _labels.Add(label1);
            _labels.Add(label2);
            _labels.Add(label3);
            _subControls.Add(button2);
            _subControls.Add(button3);
            _subControls.Add(button4);
            _subControls.Add(button5);
            _subControls.Add(button6);
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            _labels.ForEach(x => x.Visible = false);
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\Users\Guillermo Duarte\Downloads\",
                Title = "BrowseTextFiles",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "srt",
                Filter = "srt files (*.srt)|*.srt",
                FilterIndex = 1,
                RestoreDirectory = true,
            };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
                _subControls.ForEach(x => x.Visible = true);
                _labels.ForEach(x => x.Visible = false);
                var SRT = new List<string>();
                Subtitles = new List<Subtitle>();
                using (StreamReader sr = File.OpenText(textBox1.Text))
                {
                    string reading;
                    while ((reading = sr.ReadLine()) != null)
                    {
                        SRT.Add(reading);
                        if (reading == "")
                        {
                            Subtitles.Add(new Subtitle(SRT.ToArray()));
                            SRT.Clear();
                        }

                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _subControls.ForEach(x => x.Enabled = false);
            sum += finesse;
            foreach(var sub in Subtitles)
            {
                sub.AddSeconds(new TimeSpan(finesseToTicks));
            }
            Adjustlabels();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            _subControls.ForEach(x => x.Enabled = false);
            sum -= finesse;
            foreach (var sub in Subtitles)
            {
                sub.SubstractSeconds(new TimeSpan(finesseToTicks));
            }
            Adjustlabels();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            finesse /= 2;
            finesse = finesse < .25 ? .25 : finesse;
            button2.Text = '+' + finesse.ToString();
            button3.Text = '-' + finesse.ToString();

        }
        private void button5_Click(object sender, EventArgs e)
        {
            finesse *= 2;
            finesse = finesse > 8 ? 8 : finesse;
            button2.Text = '+' + finesse.ToString();
            button3.Text = '-' + finesse.ToString();   
        }
        private void Adjustlabels()
        {
            label1.Visible = false;           
            if (sum == 0)
            {
                label2.Visible = false;
            }
            else if (sum > 0)
            {
                label2.Text = $"Increased {sum} seconds";
                if (sum == 1) label2.Text = $"Increased {sum} second";
                label2.Visible = true;
            }
            else
            {
                label2.Text = $"Decreased {sum} seconds";
                if (sum == -1) label2.Text = $"Decreased {sum} second";
                label2.Visible = true;
            }
            label1.Text = "Complete";
            label1.Visible = true;
            _subControls.ForEach(x => x.Enabled = true);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            label3.Visible = false;
            using (var sw = new StreamWriter(textBox1.Text))
            { 
                foreach(var subtitle in Subtitles)
                {
                    for(var i = 0; i < subtitle.Length; i++)
                    {
                        sw.WriteLine(subtitle[i]);
                    }
                }
            }
            label3.Text = "Write Complete";
            label3.Visible = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

}

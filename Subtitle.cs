using System;
using System.Collections.Generic;

namespace SRTmodifier
{
    public class Subtitle
    {
        private List<string> _content;
        public int Length
        {
            get { return _content.Count+2; }
        }
        private TimeSpan StartTime;
        private TimeSpan EndTime;
        private string _timespan;
        private int IDnum;
        private const string _connector = "-->";

        public Subtitle(string[] rawcontent)
        {
            var timeSpans = rawcontent[1];
            var connectorIndex = timeSpans.IndexOf(_connector);
            IDnum = Int32.Parse(rawcontent[0]);
            _content = new List<string>();
            timeSpans = timeSpans.Replace(',', '.');
            var sT = timeSpans.Substring(0, connectorIndex - 1);
            sT = _completetime(sT);
            StartTime = TimeSpan.Parse(sT);
            var eT = timeSpans.Substring(connectorIndex + 4);           
            eT = _completetime(eT);           
            EndTime = TimeSpan.Parse(eT);
            for (var i = 2; i < rawcontent.Length; i++)
            {
                _content.Add(rawcontent[i]);
            }
        }
        public string this[int key]
        {
            get
            {
                if (key == 0)
                    return IDnum.ToString();
                else if (key == 1)
                {
                    _settimespan();
                    return _timespan;
                }                   
                else
                    return _content[key-2];

            }
        }
        private string _completetime(string timeSpan)
        {
            if (timeSpan.Length == 8) timeSpan += '.';
            if (timeSpan.Length < 12) return _completetime(timeSpan + '0');
            else return timeSpan;
        }
        public void AddSeconds(TimeSpan time)
        {
            StartTime += time;
            EndTime += time;
        }

        public void SubstractSeconds(TimeSpan time)
        {
            StartTime -= time;
            EndTime -= time;
        }

        private void _settimespan()
        {
            _timespan = (StartTime.ToString() + ' ' + _connector + ' ' + EndTime.ToString()).Replace('.', ',');
        }
    }
}

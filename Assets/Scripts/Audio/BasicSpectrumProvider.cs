using System;
using System.Collections.Generic;
using CSCore.DSP;


namespace Assets.Scripts.Audio
{
    public class BasicSpectrumProvider : FftProvider, ISpectrumProvider
    {
        private readonly int _sampleRate;
        private readonly List<object> _context = new List<object>();


        public BasicSpectrumProvider(int channels, int sampleRate, FftSize fftSize): base (channels, fftSize)
        {
            if(sampleRate <= 0)
            {
                throw new ArgumentOutOfRangeException("SampleRate");
            }
            _sampleRate = sampleRate;
        }

        public int GetFftBandIndex(float frequency)
        {
            int fftSize = (int)FftSize;
            double f = _sampleRate / 2.0;
            return (int)((frequency / f) * fftSize / 2);
        }
        public bool GetFftData(float[] fftResultBuffer, object context)
        {
            if (_context.Contains(context)) return false;

            _context.Add(context);
            GetFftData(fftResultBuffer);
            return true;
        }

        public new void Add(float[] samples, int count)
        {
            base.Add(samples, count);
            if (count > 0) _context.Clear();
        }

        public new void Add(float left, float right)
        {
            base.Add(left, right);
            _context.Clear();
        }

   
    }
}

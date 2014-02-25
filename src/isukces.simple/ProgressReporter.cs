using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace isukces.simple
{
    public sealed class ProgressReporter : IDisposable
    {
        #region Static Methods

        // Public Methods 

        public static ProgressReporter Coalesce(ref ProgressReporter reporter)
        {
            if (reporter == null)
                reporter = new ProgressReporter();
            return reporter;
        }

        public static double Div(int a, int b)
        {
            return PERCENT100 * a / b;
        }

        public static ProgressReporter FromAction(Action<ProgressReporterEventArgs> myAction)
        {
            ProgressReporter reporter = new ProgressReporter();
            reporter.OnProgress += new EventHandler<ProgressReporterEventArgs>((sender, args) => myAction(args));
            return reporter;
        }

        public static ProgressReporter FromAction(Action<object, ProgressReporterEventArgs> myAction)
        {
            ProgressReporter reporter = new ProgressReporter();
            reporter.OnProgress += new EventHandler<ProgressReporterEventArgs>((sender, args) => myAction(sender, args));
            return reporter;
        }

        public static ProgressReporter FromAction(Action<double, string> action)
        {
            //  public static implicit operator ProgressReporter(Action<double, string> action)
            ProgressReporter p = new ProgressReporter();
            p.OnProgress += (sender, a) =>
            {
                action(a.Percent, a.Message);
            };
            return p;
        }

        public static double Scale(double min, double max, double scaledValue)
        {
            return min + (max - min) * scaledValue / 100.0;
        }

        #endregion Static Methods

        #region Methods

        // Public Methods 

        public void Dispose()
        {
            OnProgress = null;
        }

        public void Finish(string message = null)
        {
            Report(100.0, message);
        }

        public ProgressReporter[] GetBalanced(params int[] callCount)
        {
            double[] percents;
            percents = new double[callCount.Length];
            int sum = 0;
            for (int i = 0; i < callCount.Length; i++)
            {
                sum += callCount[i];
                percents[i] = sum;
            }

            percents = percents.Select(q => q * 100.0 / percents[callCount.Length - 1]).ToArray();
            ProgressReporter[] reet = new ProgressReporter[callCount.Length];
            double last = 0;
            for (int i = 0; i < callCount.Length; i++)
            {
                reet[i] = GetScaled(last, percents[i]);
                last = percents[i];
            }
            return reet;
        }

        /// <summary>
        /// Tworzy element zależny, który forwarduje zapytanie dodając przeskalowanie
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public ProgressReporter GetScaled(double min, double max)
        {
            Func<ProgressReporterEventArgs, ProgressReporterEventArgs> f =
                (args) => args.GetScaled(min, max);
            return GetTransformed(f);
        }

        public ProgressReporter GetScaled(int etap, int zEtapow, bool addAdditionalInfo)
        {
            double delta = 100.0 / zEtapow;
            double min = (etap - 1) * delta;
            double max = etap * delta;

            Func<ProgressReporterEventArgs, ProgressReporterEventArgs> transformFunction = (args) =>
            {
                string m = args.Message;
                if (addAdditionalInfo)
                    m = string.Format("Etap {0} z {1}: {2}", etap, zEtapow, m);
                return args.GetScaled(min, max, m);
            };
            return GetTransformed(transformFunction);
        }

        public ProgressReporter GetScaledAppend(double min, double max, string prefixMessage, string separator = ": ")
        {
            Func<ProgressReporterEventArgs, ProgressReporterEventArgs> transformFunction = (args) =>
                {
                    var qq = from i in new string[] { prefixMessage, args.Message }
                             let ii = (i ?? "").Trim()
                             where !string.IsNullOrEmpty(ii)
                             select ii;
                    return args.GetScaled(min, max, string.Join(separator, qq));
                };
            return GetTransformed(transformFunction);
        }

        /// <summary>
        /// Tworzy element zależny, który forwarduje zapytanie dodając przeskalowanie
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public ProgressReporter GetScaledFormat(double min, double max, string forceMessage)
        {
            Func<ProgressReporterEventArgs, ProgressReporterEventArgs> transformFunction =
               (args) => args.GetScaled(min, max, string.Format(forceMessage, args.Message));
            return GetTransformed(transformFunction);
        }

        public ProgressReporter GetTransformed(Func<ProgressReporterEventArgs, ProgressReporterEventArgs> transformFunction)
        {
            if (transformFunction == null)
                return this;
            ProgressReporter reporter = new ProgressReporter();
            reporter.OnProgress += (sender, args) =>
            {
                if (OnProgress != null)
                    OnProgress(sender, transformFunction(args));
            };
            return reporter;
        }

        public void Report(ProgressReporterEventArgs args)
        {
            if (OnProgress == null && args != null) return;
            OnProgress(this, args);
        }

        public void Report(double Percent, string Message = null)
        {
            if (OnProgress == null) return;
            OnProgress(this, new ProgressReporterEventArgs(Percent, Message));
        }

        // string lastMessage = null;
        public void Report(int p, int total, string Message)
        {
            Report(Div(p, total), Message);
        }

        #endregion Methods

        #region Fields

        /// <summary>
        /// Liczba 100
        /// </summary>
        public const double PERCENT100 = 100;

        #endregion Fields

        #region Static Properties

        public static ProgressReporter DebugReporter
        {
            get
            {
                Action<double, string> u =
                    (p, m) => System.Diagnostics.Debug.WriteLine(string.Format("PROGRESS: {0} {1}", p, m));
                return ProgressReporter.FromAction(u);
            }
        }

        #endregion Static Properties

        #region Delegates and Events

        // Events 

        public event EventHandler<ProgressReporterEventArgs> OnProgress;

        #endregion Delegates and Events
    }
}

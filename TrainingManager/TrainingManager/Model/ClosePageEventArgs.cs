using System;

namespace TrainingManager.Model
{
    public class ClosePageEventArgs : EventArgs
    {
        public PageType Page { get; private set; }
        public ClosePageEventArgs(PageType page) => Page = page;
    }
}

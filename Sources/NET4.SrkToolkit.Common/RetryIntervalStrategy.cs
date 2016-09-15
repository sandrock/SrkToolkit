using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SrkToolkit
{
    public enum RetryIntervalStrategy
    {
        ConstantDelay,
        LinearDelay,
        ProgressiveDelay,
        ExponentialDelay,
    }
}

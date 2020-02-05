using PerformanceCounterHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMusicStore.Infrastructure
{
    [PerformanceCounterCategory
        ("MvcMusicStore",
        System.Diagnostics.PerformanceCounterCategoryType.MultiInstance,
        "MvcMusicStore")]
    public enum PerfomanceCounters
    {
        [PerformanceCounter
           ("LogIn counter",
           "Count of log in",
           System.Diagnostics.PerformanceCounterType.NumberOfItems32)]
        LogIn,
        [PerformanceCounter
           ("LogOff counter",
           "Count of log off",
           System.Diagnostics.PerformanceCounterType.NumberOfItems32)]
        LogOff,
        [PerformanceCounter
            ("Visit Home page counter",
            "Count of Home page visiting ",
            System.Diagnostics.PerformanceCounterType.NumberOfItems32)]
        VisitHomePage
    }
}
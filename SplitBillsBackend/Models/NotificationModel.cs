﻿using System;
using SplitBillsBackend.Entities;

namespace SplitBillsBackend.Models
{
    public class NotificationModel
    {
        public DateTime Date { get; set; }
        public ActionType HistoryType { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public decimal Amount { get; set; }  
    }
}

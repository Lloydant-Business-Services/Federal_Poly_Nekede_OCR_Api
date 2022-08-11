﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class GetCourseContentDto
    {
        public string NoteLink { get; set; }
        public string VideoLink { get; set; }
        public string LiveStreamLink { get; set; }
        public string TopicName { get; set; }
        public string TopicDescription { get; set; }
        public DateTime StartTime { get; set; }
        //public int NumberOfViews { get; set; }
        public string ContentTitle { get; set; }
        public long TopicId { get; set; }
        public long ContentId { get; set; }
    }
}

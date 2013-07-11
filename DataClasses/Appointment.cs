using System;

namespace TRAWebServer.DataClasses
{
    class Appointment
    {
        public string book_code { set; get; }
        public string Enter_time { set; get; }
        public string Ap_num { set; get; }
        public string app_rec_type { set; get; }
        public string app_rec_no { set; get; }
        public string app_rec_suff { set; get; }
        public string app_pat_id { set; get; }
        public string Elapsed_time { set; get; }
        public string ap_status { set; get; }
        public string ap_Notes { set; get; }
        public string esp_code { set; get; }
        public string Proc_code { set; get; }
        public string ap_ins { set; get; }
        public string app_ack { set; get; }
    }
}

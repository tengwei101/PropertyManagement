using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace PropertyManagement
{
    public class MaintenanceRequest : INotifyPropertyChanged
    {
        public string Id { get; set; }
        public string TenantName { get; set; }
        public string PropertyId { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public string SubmissionDate { get; set; }
        public string ImageUrl { get; set; }

        private string _completionDate;
        public string CompletionDate
        {
            get { return _completionDate; }
            set
            {
                if (_completionDate != value)
                {
                    _completionDate = value;
                    OnPropertyChanged("CompletionDate");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

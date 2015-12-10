using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Phytel.Services.Communication.Harness.Annotations;

namespace Phytel.Services.Communication.Harness.Model
{
    public class TestViewModel : INotifyPropertyChanged
    {
        public string XmlDocString { get; set; }

        private EmailActivityDetail _emailActivityDetail;
        public EmailActivityDetail EmailActivityDetail
        {
            get { return _emailActivityDetail; }
            set
            {
                if (value == _emailActivityDetail) return;
                _emailActivityDetail = value;
                OnPropertyChanged();
            }
        }

        private TextActivityDetail _textActivityDetail;
        public TextActivityDetail TextActivityDetail
        {
            get { return _textActivityDetail; }
            set
            {
                if (value == _textActivityDetail) return;
                _textActivityDetail = value;
                OnPropertyChanged();
            }
        }

        private TemplateDetail _templateDetail;

        public TemplateDetail TemplateDetail
        {
            get { return _templateDetail; }
            set
            {
                if(value == _templateDetail) return;
                _templateDetail = value;
                OnPropertyChanged();
            }
        }

        private string _transformResult;

        public string TransformResult
        {
            get { return _transformResult; }
            set
            {
                if(value == _transformResult) return;
                _transformResult = value;
                OnPropertyChanged();
            }
        }

        private List<ActivityMedia> _emailActivityMedia;
        public List<ActivityMedia> EmailActivityMedia
        {
            get { return _emailActivityMedia; }
            set
            {
                if (value == _emailActivityMedia) return;
                _emailActivityMedia = value;
                OnPropertyChanged();
            }
        }

        private List<ActivityMedia> _textActivityMedia;
        public List<ActivityMedia> TextActivityMedia
        {
            get { return _textActivityMedia; }
            set
            {
                if (value == _textActivityMedia) return;
                _textActivityMedia = value;
                OnPropertyChanged();
            }
        }

        // Missing Objects
        public Hashtable MissingObjects { get; set; }

        // ----------------- Activity Media (Email)
        // Facility Name
        private ActivityMedia _facilityName;
        public ActivityMedia FacilityName
        {
            get { return _facilityName; }
            set
            {
                if(value == _facilityName) return;
                _facilityName = value;
                OnPropertyChanged();
            }
        }

        // Facility Display Name
        private ActivityMedia _facilityDisplayName;
        public ActivityMedia FacilityDisplayName
        {
            get { return _facilityDisplayName; }
            set
            {
                if (value == _facilityDisplayName) return;
                _facilityDisplayName = value;
                OnPropertyChanged();
            }
        }

        // Facility Logo
        private ActivityMedia _facilityLogo;
        public ActivityMedia FacilityLogo
        {
            get { return _facilityName; }
            set
            {
                if (value == _facilityLogo) return;
                _facilityLogo = value;
                OnPropertyChanged();
            }
        }

        // Facility URL
        private ActivityMedia _facilityUrl;
        public ActivityMedia FacilityUrl
        {
            get { return _facilityUrl; }
            set
            {
                if (value == _facilityUrl) return;
                _facilityUrl = value;
                OnPropertyChanged();
            }
        }

        // Schedule Name
        private ActivityMedia _scheduleName;
        public ActivityMedia ScheduleName
        {
            get { return _scheduleName; }
            set
            {
                if (value == _scheduleName) return;
                _scheduleName = value;
                OnPropertyChanged();
            }
        }

        // Facility Email Address
        private ActivityMedia _facilityEmailAddress;
        public ActivityMedia FacilityEmailAddress
        {
            get { return _facilityEmailAddress; }
            set
            {
                if (value == _facilityEmailAddress) return;
                _facilityEmailAddress = value;
                OnPropertyChanged();
            }
        }

        // Facility Reply Address
        private ActivityMedia _facilityReplyAddress;
        public ActivityMedia FacilityReplyAddress
        {
            get { return _facilityReplyAddress; }
            set
            {
                if (value == _facilityReplyAddress) return;
                _facilityReplyAddress = value;
                OnPropertyChanged();
            }
        }

        // Appointment Specific Message
        private ActivityMedia _appointmentSpecificMessage;
        public ActivityMedia AppointmentSpecificMessage
        {
            get { return _appointmentSpecificMessage; }
            set
            {
                if (value == _appointmentSpecificMessage) return;
                _appointmentSpecificMessage = value;
                OnPropertyChanged();
            }
        }

        // ----------------- Activity Media (Text)
        // Facility Name
        private ActivityMedia _facilityDisplayNameTxt;
        public ActivityMedia TextFacilityName
        {
            get { return _facilityDisplayNameTxt; }
            set
            {
                if (value == _facilityDisplayNameTxt) return;
                _facilityDisplayNameTxt = value;
                OnPropertyChanged();
            }
        }

        // Facility Display Name
        private ActivityMedia _textScheduleName;
        public ActivityMedia TextScheduleName
        {
            get { return _textScheduleName; }
            set
            {
                if (value == _textScheduleName) return;
                _textScheduleName = value;
                OnPropertyChanged();
            }
        }

        // Notify property changed
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

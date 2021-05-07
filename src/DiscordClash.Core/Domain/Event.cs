using System;
using System.ComponentModel.DataAnnotations;

namespace DiscordClash.Core.Domain
{
    public class Event
    {
        public Guid Id { get; protected set; }
        public string FullName { get; protected set; }
        public DateTime StarTime { get; protected set; }
        public DateTime EndTime { get; protected set; }
        public string Country { get; protected set; }
        public string City { get; protected set; }
        public bool IsFree { get; protected set; }
        public Game GameCode { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }

        public Event() { }

        public Event(Guid id, string fullName, DateTime starTime, DateTime endTime, string country, string city, bool isFree, Game gameCode)
        {
            Id = id;
            SetFullName(fullName);
            SetPeriodTimes(starTime, endTime);
            Country = country;
            City = city;
            IsFree = isFree;
            GameCode = gameCode;

            CreatedAt = DateTime.Now;
            UpdatedAt = null;
        }

        public void SetFullName(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
            {
                throw new ValidationException("Event's full name cannot be empty.");
            }

            FullName = fullName;
            UpdatedAt = DateTime.Now;
        }

        public void SetPeriodTimes(DateTime startTime, DateTime endTime)
        {
            if (startTime > endTime)
            {
                throw new ValidationException("Event's start date cannot be greater than end date.");
            }

            StarTime = startTime;
            EndTime = endTime;
            UpdatedAt = DateTime.Now;
        }
    }
}

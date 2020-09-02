﻿using gs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sutro.Core.Models.Profiles;
using Sutro.PathWorks.Plugins.API.Settings;
using Sutro.PathWorks.Plugins.Core.UserSettings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sutro.PathWorks.Plugins.Core.Settings
{
    public abstract class ProfileManager<TProfile> : IProfileManager<TProfile> where TProfile : IProfile
    {
        public abstract List<TProfile> FactoryProfiles { get; }

        public abstract UserSettingCollectionBase<TProfile> UserSettings { get; }

        IUserSettingCollection IProfileManager<TProfile>.UserSettings => UserSettings;

        public void ApplyJSON(TProfile settings, string json)
        {
            JsonConvert.PopulateObject(json, settings, SerializerSettings());
        }

        public void ApplyKeyValuePair(TProfile settings, string keyValue)
        {
            // TODO: Make this more strict to avoid converting values unintentionally
            var sFormatted = StringUtil.FormatSettingOverride(keyValue);
            JsonConvert.PopulateObject(sFormatted, settings, SerializerSettings());
        }

        public TProfile DeserializeJSON(string json)
        {
            var profile = JsonConvert.DeserializeObject<TProfile>(json, SerializerSettings());
            return profile;
        }

        public string SerializeJSON(TProfile settings)
        {
            return JsonConvert.SerializeObject(settings, settings.GetType(), SerializerSettings());
        }

        protected virtual JsonSerializerSettings SerializerSettings()
        {
            return new JsonSerializerSettings()
            {
                MissingMemberHandling = MissingMemberHandling.Error,
                TypeNameHandling = TypeNameHandling.Auto
            };
        }

        private TProfile CreateProfileFromTypeName(string typestring)
        {
            foreach (var profile in FactoryProfiles)
            {
                if (profile.GetType().Name == typestring.Split(".")[^1])
                {
                    return (TProfile)profile.Clone();
                }
            }
            throw new InvalidCastException($"Couldn't find factory setting of type {typestring}.");
        }
    }
}
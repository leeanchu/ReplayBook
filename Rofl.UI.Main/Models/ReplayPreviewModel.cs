﻿using Rofl.Reader.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rofl.UI.Main.Models
{
    public class ReplayPreviewModel
    {

        public ReplayPreviewModel(ReplayFile replayFile, DateTimeOffset creationDate, bool newFile = false)
        {
            if (replayFile == null) { throw new ArgumentNullException(nameof(replayFile)); }

            // Copy all the replay file fields
            Name = replayFile.Name;
            GameDuration = replayFile.GameDuration;
            GameVersion = replayFile.GameVersion;
            MatchId = replayFile.MatchId;
            MapName = replayFile.MapName;
            IsBlueVictorious = replayFile.IsBlueVictorious;

            // Set new fields
            CreationDate = creationDate;
            IsNewFile = newFile;

            BluePreviewPlayers = (from bplayer in replayFile.BluePlayers
                                  select new PlayerPreviewModel(bplayer)).ToList();

            RedPreviewPlayers = (from rplayer in replayFile.RedPlayers
                                 select new PlayerPreviewModel(rplayer)).ToList();
        }

        public string Name { get; private set; }

        public TimeSpan GameDuration { get; private set; }

        public string GameVersion { get; private set; }

        public string MatchId { get; private set; }

        public string MapName { get; private set; }

        public bool IsBlueVictorious { get; private set; }

        public DateTimeOffset CreationDate { get; set; }

        public bool IsNewFile { get; private set; }

        public string GameLengthString
        {
            get {
                return $"{(int)GameDuration.TotalMinutes} m {GameDuration.Seconds} s";
            }
        }

        public IList<PlayerPreviewModel> BluePreviewPlayers { get; private set; }

        public IList<PlayerPreviewModel> RedPreviewPlayers { get; private set; }
    }
}
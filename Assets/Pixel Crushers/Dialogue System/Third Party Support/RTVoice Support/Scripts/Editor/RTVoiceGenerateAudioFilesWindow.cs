using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Crosstales.RTVoice;
using Crosstales.RTVoice.Model;

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// Editor utility to use RT-Voice to generate and save audio files for the lines of
    /// dialogue in a dialogue database. To help select the right voices, the actors in
    /// the database may have these custom fields:
    /// 
    /// - VoiceName: An available voice name as reported by RT-Voice. Can also add localized versions such as "VoiceName es" for Spanish.
    /// - Gender: "Male" or "Female".
    /// - MinAge & MaxAge: Voice age ranage to use. If both are zero, any age is acceptable.
    /// </summary>
    public class RTVoiceGenerateAudioFilesWindow : EditorWindow
    {

        [MenuItem("Tools/Pixel Crushers/Dialogue System/Third Party Support/RT-Voice/Save Audio Files...")]
        public static void OpenWindow()
        {
            GetWindow<RTVoiceGenerateAudioFilesWindow>().Show();
        }

        [SerializeField]
        private DialogueDatabase m_database;

        [SerializeField]
        private EntrytagFormat m_entrytagFormat = EntrytagFormat.ActorName_ConversationID_EntryID;

        [SerializeField]
        private string m_languageCode;

        [SerializeField]
        private string m_folder;

        private void OnEnable()
        {
            titleContent.text = "Save Audio";
            if (m_database == null) m_database = EditorTools.FindInitialDatabase();
            var dsc = FindObjectOfType<DialogueSystemController>();
            if (dsc != null && dsc.displaySettings != null)
            {
                if (dsc.displaySettings.cameraSettings != null) m_entrytagFormat = dsc.displaySettings.cameraSettings.entrytagFormat;
                if (dsc.displaySettings.localizationSettings != null && string.IsNullOrEmpty(m_languageCode)) m_languageCode = dsc.displaySettings.localizationSettings.language;
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.HelpBox("This tool uses RT-Voice to generate and save audio files for the dialogue entries in a dialogue database.", MessageType.Info);
            m_database = EditorGUILayout.ObjectField("Database", m_database, typeof(DialogueDatabase), false) as DialogueDatabase;
            m_entrytagFormat = (EntrytagFormat)EditorGUILayout.EnumPopup("Entrytag Format", m_entrytagFormat);
            EditorGUI.BeginDisabledGroup(m_database == null);
            if (GUILayout.Button("Save Files...")) SaveFiles();
            EditorGUI.EndDisabledGroup();
        }

        private void SaveFiles()
        {
            m_folder = EditorUtility.OpenFolderPanel("Save Audio Files", m_folder, string.Empty);
            if (string.IsNullOrEmpty(m_folder)) return;
            var speaker = FindObjectOfType<Speaker>();
            var isTempSpeaker = (speaker == null);
            if (speaker == null)
            {
                EditorApplication.ExecuteMenuItem("Tools/RTVoice/Add/RTVoice");
                speaker = FindObjectOfType<Speaker>();
                if (speaker == null) return;
            }
            try
            {
                ProcessDatabase(speaker);
            }
            finally
            {
                if (isTempSpeaker) DestroyImmediate(speaker);
            }
        }

        private void ProcessDatabase(Speaker speaker)
        {
            if (speaker == null || m_database == null) return;
            Localization.Language = m_languageCode;
            var voices = FindVoices();
            try
            {
                for (int c = 0; c < m_database.conversations.Count; c++)
                {
                    var progress = c / m_database.conversations.Count;
                    var conversation = m_database.conversations[c];
                    var conversationTitle = conversation.Title;
                    var cancel = EditorUtility.DisplayCancelableProgressBar(conversationTitle, string.Empty, progress);
                    if (cancel) return;
                    foreach (var entry in conversation.dialogueEntries)
                    {
                        var entrytag = m_database.GetEntrytag(conversation, entry, m_entrytagFormat);
                        var text = entry.subtitleText;
                        cancel = EditorUtility.DisplayCancelableProgressBar(conversationTitle, entrytag, progress);
                        if (cancel) return;
                        SaveAudioFileForText(voices[entry.ActorID], text, entrytag);
                    }
                }
            }
            finally
            {
                AssetDatabase.Refresh();
                EditorUtility.ClearProgressBar();
                EditorUtility.DisplayDialog("Audio Files Saved", "If you don't see the files immediately, right-click on this folder and select Reimport:\n" + m_folder, "OK");
            }
        }

        private Dictionary<int, Voice> FindVoices()
        {
            var voices = new Dictionary<int, Voice>();
            if (m_database == null) return voices;
            foreach (var actor in m_database.actors)
            {

                var voice = GetVoice(actor.LookupLocalizedValue("VoiceName"), actor.LookupValue("Gender"), actor.LookupInt("MinAge"), actor.LookupInt("MaxAge"));
                voices.Add(actor.id, voice);
            }
            return voices;
        }

        public Voice GetVoice(string voiceName, Crosstales.RTVoice.Model.Enum.Gender gender, int minAge, int maxAge)
        {
            return GetVoice(voiceName, gender.ToString(), minAge, maxAge);
        }

        public Voice GetVoice(string voiceName, string gender, int minAge, int maxAge)
        {
            var culture = Localization.Language;
            var availableVoices = string.IsNullOrEmpty(culture) ? Speaker.Instance.Voices : Speaker.Instance.VoicesForCulture(culture);
            foreach (var availableVoice in availableVoices)
            {
                var matchesName = string.IsNullOrEmpty(voiceName) || string.Equals(voiceName, availableVoice.Name);
                var matchesGender = string.Equals(gender, availableVoice.Gender.ToString(), System.StringComparison.OrdinalIgnoreCase);
                var age = Tools.StringToInt(availableVoice.Age);
                var matchesAge = (minAge == 0 && maxAge == 0) || (minAge <= age && age <= maxAge);
                if (matchesName && matchesGender && matchesAge) return availableVoice;
            }
            return (availableVoices != null && availableVoices.Count > 0) ? availableVoices[0] : null;
        }

        private void SaveAudioFileForText(Voice voice, string text, string filename)
        {
            if (string.IsNullOrEmpty(text)) return;
            var path = m_folder + "/" + filename + Speaker.Instance.AudioFileExtension;
            Speaker.Instance.Generate(text, path, voice);
        }
    }
}
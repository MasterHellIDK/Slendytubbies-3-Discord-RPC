using MelonLoader;
using UnityEngine.SceneManagement;

namespace ST3
{
    public class Script : MelonMod
    {
        public Discord.Discord _discord;

        public static string _roomNow, _roomOld;
        public static int _playerSize, _playersCount;

        public override void OnApplicationStart()
        {
            DiscordLibraryLoader.LoadLibrary();
            _discord = new Discord.Discord(1053670755939921961, (ulong)Discord.CreateFlags.Default);

            _roomNow = "MainMenu";

            var activityManager = _discord.GetActivityManager();
            var activity = new Discord.Activity
            {
                State = "",
                Details = _roomNow,
                Assets = { LargeImage = "st3_img", },
            };

            activityManager.UpdateActivity(activity, (res) => { });
        }

        public override void OnUpdate()
        {
            if (PhotonNetwork.inRoom)
            {
                _playerSize = PhotonNetwork.room.MaxPlayers;
            }

            if (PhotonNetwork.inRoom && PhotonNetwork.room.Name != _roomOld)
            {
                _roomNow = PhotonNetwork.room.Name;
                _roomOld = _roomNow;
                UpdateActivity(_discord, IsRoomPrivate(), RoomName(), _playerSize);
            }
            else if (PhotonNetwork.inRoom && PhotonNetwork.room.Name == _roomOld)
            {
                UpdateActivity(_discord, IsRoomPrivate(), RoomName(), _playerSize);
            }

            _discord.RunCallbacks();
        }

        public string RoomName()
        {
            string SceneRoom = SceneManager.GetActiveScene().name;
            return SceneRoom;
        }

        public string IsRoomPrivate()
        {
            if (PhotonNetwork.room.IsVisible && PhotonNetwork.inRoom)
            {
                return "Public";
            }
            else
            {
                return "Private";
            }
        }

        static void UpdateActivity(Discord.Discord discord, string state, string Details, int size)
        {
            var activityManager = discord.GetActivityManager();
            var lobbyManager = discord.GetLobbyManager();
            var activity = new Discord.Activity
            {
                State = state,
                Details = Details,
                Assets = { LargeImage = "st3_img", },
                Party = { Id = "masterhell", Size = { CurrentSize = size, MaxSize = PhotonNetwork.room.MaxPlayers, }, },
                Instance = true,
            };

            activityManager.UpdateActivity(activity, result => {});
        }
    }
}

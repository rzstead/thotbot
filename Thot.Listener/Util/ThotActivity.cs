using Discord;

namespace Thot.Listener.Util
{
    public class ThotActivity : IActivity
    {
        public string Name => "over all my children.";

        public ActivityType Type => ActivityType.Watching;

        public ActivityProperties Flags => ActivityProperties.None;

        public string Details => "over all my children.";
    }
}
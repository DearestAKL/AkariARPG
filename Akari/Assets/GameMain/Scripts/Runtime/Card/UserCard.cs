namespace Akari
{
    public class UserCard
    {
        public UserCard(int id,string name,string content,CardType type)
        {
            this.id = id;
            this.name = name;
            this.content = content;
            this.type = type;
        }

        /// <summary>
        /// 从2001 开始
        /// </summary>
        public int id;
        public string name;
        public string content;
        public CardType type;
    }
}

namespace Akari
{
    public class PlayerData : EntityData
    {
        public int playerId;
        public string playerName;
        public int playerPos;

        public PlayerData(int entityId, int typeId,int ownerId, string name, int pos) : base(entityId, typeId)
        {
            this.playerId = ownerId;
            this.playerName = name;
            this.playerPos = pos;
        }
    }
}

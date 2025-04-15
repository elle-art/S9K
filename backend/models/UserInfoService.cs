public class UserInfoServices
{
    public UserInfo createUserInfo(UserInfo u)
    {
        return null;
    }

    public void respondToInvite(bool response)
    {
    }

    public void saveUser(ref UserInfo curUser)
    {
    }

    public void updateUser(ref UserInfo curUser)
    {
    }

    public void getUserInfo()
    {
    }

    public Availability getUserAvailability(ref UserInfo curUser)
    {
        return curUser.userAvailability;
    }

    public string getName(ref UserInfo curUser)
    {
        return curUser.displayName;
    }
}
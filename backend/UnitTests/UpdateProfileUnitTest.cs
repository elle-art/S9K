using System.Threading.Tasks;
using Xunit;
using Backend.Models;
using Backend.Services;

public class UserInfoServiceTests
{
    [Fact]
    public async Task UpdateUserInfo_OverwritesExistingRecord()
    {
        // ─── Arrange ─────────────────────────────────────────────────────────
        var avail = new Availability();                 // empty dummy schedule
        var user  = await UserInfoServices.CreateUserInfo(
                        "Alice", avail,
                        new(), new(), new());           // other lists empty

        // verify the default goal is in the DB
        var original = await UserInfoServices.GetUserInfo("Alice");
        Assert.Equal("schedule more!", original!.WeeklyGoal);

        // ─── Act: change a field and call the update method ─────────────────
        user.WeeklyGoal = "finish capstone early";
        UserInfoServices.UpdateUserInfo(user);          // async‑void fire‑and‑forget

        // ─── Assert: read it back and confirm the change took effect ───────
        var updated = await UserInfoServices.GetUserInfo("Alice");

        Assert.NotNull(updated);
        Assert.Equal("finish capstone early", updated!.WeeklyGoal);
    }
}
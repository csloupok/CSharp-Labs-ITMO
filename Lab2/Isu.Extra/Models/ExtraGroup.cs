using Isu.Entities;
using Isu.Extra.Tools.Exceptions;
using Isu.Models;

namespace Isu.Extra.Models;

public class ExtraGroup : Group<ExtraStudent>
{
    private Schedule? _schedule;

    public ExtraGroup(GroupName groupName)
        : base(groupName)
    {
        _schedule = null;
    }

    public Schedule? Schedule => _schedule;
    public char Faculty => GroupName.Name[0];

    public void SetSchedule(Schedule schedule)
    {
        _schedule = schedule ?? throw ScheduleException.NullArgument();
    }
}
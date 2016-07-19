class Indicator 
{
    constructor(Id, Name, Type, Group = null, Image = false) 
    {
        this.Id = Id;
        this.Name = Name;
        this.Type = Type;
        this.Image = Image;
        this.Group = Group;
    };
};
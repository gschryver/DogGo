namespace DogGo.Models.ViewModels
{
    public class WalkerProfileViewModel
    {
        public Walk NewWalk { get; set; }
        public Walker CurrentWalker { get; set; }
        public List<Walker> Walkers { get; set; }
        public List<Dog> Dogs { get; set; }
        public List<Walk> Walks { get; set; }
    }
}

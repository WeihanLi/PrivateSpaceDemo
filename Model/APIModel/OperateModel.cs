namespace Model.APIModel
{
    public class MoodOperateModel
    {
        public User Member { get; set; }

        public ViewModel.MoodViewModel Mood { get; set; }
    }

    public class DiaryOperateModel
    {
        public User Member { get; set; }

        public ViewModel.DiaryViewModel Diary { get; set; }
    }
}

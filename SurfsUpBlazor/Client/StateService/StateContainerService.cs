using SurfsUpLibrary.Models;

namespace SurfsUpBlazor.Client.StateService
{
    public class StateContainerService
    {
        public Board Board { get; set; }

        public event Action OnStateChange;

        public void SetBoard(Board Board)
        {
            this.Board = Board;
            NotifyStateChanged();
        }

        private void NotifyStateChanged()
        {
            OnStateChange?.Invoke();
        }
    }
}
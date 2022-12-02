using SurfsUpLibrary.Models;

namespace SurfsUpBlazor.Client.StateService
{
    public class StateContainerService
    {
        public Board board { get; set; }

        public event Action OnStateChange;

        public void SetBoard(Board board)
        {
            this.board = board;
            NotifyStateChanged();
        }

        private void NotifyStateChanged()
        {
            OnStateChange?.Invoke();
        }
    }
}
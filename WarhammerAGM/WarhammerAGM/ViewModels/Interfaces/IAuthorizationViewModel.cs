using Model;
using System.Windows.Input;

namespace ViewModels
{
    /// <summary>Служба авторизации.</summary>
    public interface IAuthorizationViewModel
    {
        /// <summary>Текущее состояние авторизации.</summary>
        AuthorizationStatus CurrentStatus { get; }

        /// <summary>Текущий пользователь.</summary>
        /// <remarks>Может быть отлично от <see langword="null"/>
        /// только после успешной авторизации.</remarks>
        IUser? CurrentUser { get; }

        /// <summary>Команда попытки авторизации.</summary>
        RelayCommand Authorize { get; }

        /// <summary>Команда попытки гостевого входа.</summary>
        RelayCommand Guest { get; }

         /// <summary>Команда попытки выхода из авторизации.</summary>
        RelayCommand Exit { get; }
    }

}

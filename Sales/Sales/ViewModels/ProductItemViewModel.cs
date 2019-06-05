namespace Sales.ViewModels
{
    using System;
    using System.Linq;
    using System.Windows.Input;
    using Common.Models;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Services;
    using Views;
    using Xamarin.Forms;

    public class ProductItemViewModel : Product
    {
        #region Attributes
        private ApiService apiService;
        #endregion

        #region Constructors
        public ProductItemViewModel()
        {
            this.apiService = new ApiService();
        }
        #endregion

        #region Commands

        public ICommand EditProductCommand
        {
            get
            {
                return new RelayCommand(EditProduct);
            }
        }

        private async void EditProduct()
        {
            //Crear la instancia y ligarla a la ViewModel
            MainViewModel.GetInstance().EditProduct = new EditProductViewModel(this);
            //Abrir la página de Edición de Productos
            await Application.Current.MainPage.Navigation.PushAsync(new EditProductPage());
        }

        public ICommand DeleteProductCommand
        {
            get
            {
                return new RelayCommand(DeleteProduct);
            }
        }

        private async void DeleteProduct()
        {
            var answer = await Application.Current.MainPage.DisplayAlert(
                Languages.Confirm,
                Languages.DeleteConfirmation,
                Languages.Yes,
                Languages.No);
            if(!answer)
            {
                return;
            }

            //Validar conexión a internet
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                return;
            }

            //Ir al API a realizar la operación de borrado
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlProductsController"].ToString();
            var response = await this.apiService.Delete(url, prefix, controller, this.ProductId);
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            //Una vez borrado el producto, se llama al Singleton para refrescar la clase borrando el producto afectado
            var productsViewModel = ProductsViewModel.GetInstance();
            var deletedProduct = productsViewModel.Products.Where(p => p.ProductId == this.ProductId).FirstOrDefault();
            if (deletedProduct != null)
            {
                productsViewModel.Products.Remove(deletedProduct);
            }
        }

        #endregion
    }
}

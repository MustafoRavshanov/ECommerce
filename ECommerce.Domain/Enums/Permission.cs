using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Enums;

public enum Permission
{
    ProductsView,
    ProductsCreate,
    ProductsEdit,
    ProductsDelete,

    OrdersView,
    OrdersEdit,
    OrdersCancel,

    UsersView,
    UsersBan,

    CategoriesManage,
}

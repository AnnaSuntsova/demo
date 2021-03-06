﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NorthwindDAL {
    using System;
    
    
    /// <summary>
    ///   Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
    /// </summary>
    // Этот класс создан автоматически классом StronglyTypedResourceBuilder
    // с помощью такого средства, как ResGen или Visual Studio.
    // Чтобы добавить или удалить член, измените файл .ResX и снова запустите ResGen
    // с параметром /str или перестройте свой проект VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("NorthwindDAL.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Перезаписывает свойство CurrentUICulture текущего потока для всех
        ///   обращений к ресурсу с помощью этого класса ресурса со строгой типизацией.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на delete from [Order Details] where OrderId = @OrderID; delete from Orders where OrderId = @OrderID.
        /// </summary>
        internal static string DeleteOrderInfo {
            get {
                return ResourceManager.GetString("DeleteOrderInfo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на select OrderID, OrderDate, ShippedDate from dbo.Orders where OrderId = @id;  select OrderId, Quantity, t1.UnitPrice, ProductName, t1.ProductId, Discount from dbo.[Order Details] t1 left outer join dbo.[Products] t2 on t1.ProductID = t2.ProductID where OrderId = @id.
        /// </summary>
        internal static string GetOrderInfoQuery {
            get {
                return ResourceManager.GetString("GetOrderInfoQuery", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на insert into dbo.[Order Details] values (@ordId, @prodId, @unitPrice, @quantity, @discount).
        /// </summary>
        internal static string InsertIntoOrderDetails {
            get {
                return ResourceManager.GetString("InsertIntoOrderDetails", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на insert into dbo.Orders (OrderDate, ShippedDate) values (@ordDate, @shipDate); SET @newId = SCOPE_IDENTITY();.
        /// </summary>
        internal static string InsertOrderGetId {
            get {
                return ResourceManager.GetString("InsertOrderGetId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на SET IDENTITY_INSERT Orders ON; insert into Orders (OrderId, OrderDate, ShippedDate) values (@ordId, @ordDate, @shipDate); SET IDENTITY_INSERT Orders OFF.
        /// </summary>
        internal static string InsertOrderWithTheSameId {
            get {
                return ResourceManager.GetString("InsertOrderWithTheSameId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на select OrderID, OrderDate, ShippedDate from dbo.Orders.
        /// </summary>
        internal static string SimpleSelectFromOrders {
            get {
                return ResourceManager.GetString("SimpleSelectFromOrders", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на update dbo.Orders set OrderDate = cast(GETDATE() AS date) where OrderID = @id.
        /// </summary>
        internal static string UpdateOrderDate {
            get {
                return ResourceManager.GetString("UpdateOrderDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на update dbo.Orders set ShippedDate = cast(GETDATE() AS date) where OrderID = @id.
        /// </summary>
        internal static string UpdateShippedDate {
            get {
                return ResourceManager.GetString("UpdateShippedDate", resourceCulture);
            }
        }
    }
}

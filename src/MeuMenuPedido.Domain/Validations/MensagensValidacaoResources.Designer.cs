﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MeuMenuPedido.Domain.Validations {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class MensagensValidacaoResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal MensagensValidacaoResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MeuMenuPedido.Domain.Validations.MensagensValidacaoResources", typeof(MensagensValidacaoResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
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
        ///   Looks up a localized string similar to Deve-se informar ao menos um produto no pedido.
        /// </summary>
        internal static string DeveInformarProdutoNoPedido {
            get {
                return ResourceManager.GetString("DeveInformarProdutoNoPedido", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Deve-se obrigatoriamente informar o pedido.
        /// </summary>
        internal static string IdentificadorDoPedidoObrigatorio {
            get {
                return ResourceManager.GetString("IdentificadorDoPedidoObrigatorio", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Deve-se obrigatoriamente informar a mesa.
        /// </summary>
        internal static string MesaObrigatoria {
            get {
                return ResourceManager.GetString("MesaObrigatoria", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Deve-se obrigatoriamente informar o produto.
        /// </summary>
        internal static string ProdutoObrigatorio {
            get {
                return ResourceManager.GetString("ProdutoObrigatorio", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Deve-se informar a quantidade do produto superior a zero.
        /// </summary>
        internal static string QuantidadeDoProdutoDeveSerSuperiorAZero {
            get {
                return ResourceManager.GetString("QuantidadeDoProdutoDeveSerSuperiorAZero", resourceCulture);
            }
        }
    }
}

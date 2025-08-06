﻿// -------------------------------------------------------------------------------------------------
// Modified by Jason La (2019).
// Comment out ASP.NET MVC specific code. Rename class and file name from
// NinjectMvcHttpApplicationPlugin to NinjectWebFormsHttpApplicationPlugin for clarity.
// 
// This is only necessary if you have a pure Web Forms application. If you have a web app that is a
// mixture of Web Forms and MVC5, this class is not necessary.
// -------------------------------------------------------------------------------------------------

// -------------------------------------------------------------------------------------------------
// <copyright file="NinjectMvcHttpApplicationPlugin.cs" company="Ninject Project Contributors">
//   Copyright (c) 2010 bbv Software Services AG. All rights reserved.
//   Copyright (c) 2010-2017 Ninject Project Contributors. All rights reserved.
//
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
//   You may not use this file except in compliance with one of the Licenses.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//   or
//       http://www.microsoft.com/opensource/licenses.mspx
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// -------------------------------------------------------------------------------------------------


using Ninject;
using Ninject.Activation;
using Ninject.Components;
using Ninject.Web.Common;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;

namespace Web.App_Start
{
    /// <summary>
    /// The web plugin implementation for MVC
    /// </summary>
    public class NinjectWebFormsHttpApplicationPlugin : NinjectComponent, INinjectHttpApplicationPlugin
    {
        /// <summary>
        /// The ninject kernel.
        /// </summary>
        private readonly IKernel kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectWebFormsHttpApplicationPlugin"/> class.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        public NinjectWebFormsHttpApplicationPlugin(IKernel kernel)
        {
            this.kernel = kernel;
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            ModelValidatorProviders.Providers.Remove(ModelValidatorProviders.Providers.OfType<DataAnnotationsModelValidatorProvider>().Single());
            //DependencyResolver.SetResolver(this.CreateDependencyResolver());
            //RemoveDefaultAttributeFilterProvider();
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
        }

        /// <summary>
        /// Gets the request scope.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The request scope.</returns>
        public object GetRequestScope(IContext context)
        {
            return HttpContext.Current;
        }

        ///// <summary>
        ///// Creates the controller factory that is used to create the controllers.
        ///// </summary>
        ///// <returns>The created controller factory.</returns>
        //protected IDependencyResolver CreateDependencyResolver()
        //{
        //    return this.kernel.Get<IDependencyResolver>();
        //}

        ///// <summary>
        ///// Removes the default attribute filter provider.
        ///// </summary>
        //private static void RemoveDefaultAttributeFilterProvider()
        //{
        //    var oldFilter = FilterProviders.Providers.Single(f => f is FilterAttributeFilterProvider);
        //    FilterProviders.Providers.Remove(oldFilter);
        //}
    }
}
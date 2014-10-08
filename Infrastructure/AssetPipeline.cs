using System.Web.Optimization;
using BundleTransformer.Core.Builders;
using BundleTransformer.Core.Orderers;
using BundleTransformer.Core.Transformers;

namespace OrangeCMS.Infrastructure
{
    internal class AssetPipeline
    {
        static readonly NullOrderer nullOrderer;
        static readonly NullBuilder nullBuilder;
        static readonly StyleTransformer styleTransformer;
        static readonly ScriptTransformer scriptTransformer;

        static AssetPipeline()
        {
            nullBuilder = new NullBuilder();
            styleTransformer = new StyleTransformer();
            nullOrderer = new NullOrderer();
            scriptTransformer = new ScriptTransformer();
        }

        public static void BundleCss(BundleCollection bundles)
        {
            bundles.Add(CreateStyleBundle("~/bundles/css/libraries",
                "~/Libraries/bootstrap.css",
                "~/Libraries/bootstrap-theme.css",
                "~/Libraries/ui-bootstrap-custom.css",
                "~/Libraries/bootstrap-responsive.css",
                "~/Libraries/ngDialog/css/ngDialog.css",
                "~/Libraries/ngDialog/css/ngDialog-theme-plain.css",
                "~/Libraries/ngDialog/css/ngDialog-theme-default.css",
                "~/Libraries/css3-buttons.css"));

            bundles.Add(CreateStyleBundle("~/bundles/css/common",
                "~/Modules/Common/Styles/Reset.css",
                "~/Modules/Common/Styles/Navigation.css",
                "~/Modules/Common/Styles/Scrollbars.css",
                "~/Modules/Common/Styles/Application.css"));

            bundles.Add(CreateModuleStyleBundle("~/bundles/css/cms", "~/Modules/CMS", "*.css"));

            bundles.Add(CreateModuleStyleBundle("~/bundles/css/security", "~/Modules/Security", "*.css"));
        }

        public static void BundleJs(BundleCollection bundles)
        {
            bundles.Add(CreateJsBundle("~/bundles/js/libraries",
                "~/Libraries/jquery/jquery.min.js",
                "~/Libraries/angular/angular-file-upload-shim.min.js",
                "~/Libraries/angular/angular.min.js",
                "~/Libraries/angular/angular-ui-router.min.js",
                "~/Libraries/lodash.js",
                "~/Libraries/ngDialog/ngDialog.min.js",
                "~/Libraries/angular/angular-google-maps.js",
                "~/Libraries/angular/angular-local-storage.js",
                "~/Libraries/angular/angular-file-upload.min.js",
                "~/Libraries/bootstrap/ui-bootstrap-custom-0.10.0.min.js",
                "~/Libraries/bootstrap/ui-bootstrap-custom-tpls-0.10.0.min.js"));

            bundles.Add(CreateModuleJsBundle("~/bundles/js/common", "~/Modules/Common"));

            bundles.Add(CreateModuleJsBundle("~/bundles/js/import", "~/Modules/Import"));

            bundles.Add(CreateModuleJsBundle("~/bundles/js/cms", "~/Modules/CMS"));

            bundles.Add(CreateModuleJsBundle("~/bundles/js/security", "~/Modules/Security"));
        }

        private static Bundle CreateStyleBundle(string bundleName, params string[] files)
        {
            return CreateStyleBundle(new StyleBundle(bundleName).Include(files));
        }

        private static Bundle CreateJsBundle(string bundleName, params string[] files)
        {
            return CreateJsBundle(new Bundle(bundleName).Include(files));
        }

        private static Bundle CreateJsBundle(Bundle bundle)
        {
            bundle.Builder = nullBuilder;
            bundle.Orderer = nullOrderer;
            bundle.Transforms.Add(scriptTransformer);
            return bundle;
        }

        private static Bundle CreateModuleJsBundle(string bundleName, string moduleDirectory)
        {
            return CreateJsBundle(new Bundle(bundleName).IncludeDirectory(moduleDirectory, "*.js", true));
        }

        private static Bundle CreateModuleStyleBundle(string bundleName, string moduleDirectory, string pattern)
        {
            return CreateStyleBundle(new StyleBundle(bundleName).IncludeDirectory(moduleDirectory,pattern, true));
        }

        private static Bundle CreateStyleBundle(Bundle bundle)
        {
            bundle.Builder = nullBuilder;
            bundle.Orderer = nullOrderer;
            bundle.Transforms.Add(styleTransformer);
            return bundle;
        }
    }
}

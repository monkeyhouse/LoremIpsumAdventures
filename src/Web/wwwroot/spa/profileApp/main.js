// Write your Javascript code.

export function configure(aurelia){
    aurelia.use
       .standardConfiguration()
       .developmentLogging();
    //.defaultBindingLanguage()
    //.defaultResources()
    //.history()
    //.router()
    //.eventAggregator()

    aurelia.start().then(a => a.setRoot('p/app'));
}
// Write your Javascript code.
import {inject} from 'aurelia-framework';  
import {Router} from 'aurelia-router';

@inject(Router)
export class Page{

    
    constructor(router){
        this.completedText = ''
        this.router = router;
    }

    activate(params){

        var storyId = $('#StoryId').val();
        var pageId = params.pageId;
        var url = `/api/Read/Page/${storyId}/${pageId}`;

        return $.get( url ).success(
            results => {
                Object.assign(this, results);
                
                //endgame
                if ( results.actions.length == 0){
                    this.initCompleteCountdown();
                }
            }
        );

    }

    //take user to stats & continue page
    initCompleteCountdown(){
        this.completedText = '...';
        var count = 0;
        var limit = 20;
        var interval = 
            window.setInterval(
                t => {
                 
                    this.completedText = 'Redirecting in in ' + (limit - count++) + ' seconds';
               
                    if (count >= limit){
                        //navigate
                        console.log('Redirecting to details');
                        this.redirectAfterComplete();
                    }
            },
        1000);        
    }
    
    redirectAfterComplete(){
        var storyId = $('#StoryId').val();
        window.location = '/Read/Details/' + storyId;
    }

    // details screen should have ability to review game
    // & see reviews

    next(pageId){
        this.router.navigateToRoute('page', {pageId : pageId});
    }


}
// Write your Javascript code.
import {inject} from 'aurelia-framework';
import {Router} from 'aurelia-router';

@inject(Router)
export class Cover{


    constructor(router){

        this.router = router;

        // use moment to process it
        // moment().fromNow();

        var storyId = $('#StoryId').val();
        this.url = '/Api/Read/Jacket/' + storyId;
    }

    activate(){
        return $.get( this.url ).success(
            response => Object.assign(this, response));
    }

    attached(){
        $(document).one('keypress',
             (e) => {  this.next() })
    }

    next(){
        var pageOneId = $('#PageOneId').val();
        this.router.navigateToRoute('page', { pageId: 123 });
    }
}

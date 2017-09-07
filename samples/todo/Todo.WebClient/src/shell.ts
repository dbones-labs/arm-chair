import { PipelineStep, RouterConfiguration, Router } from 'aurelia-router';
import { autoinject } from 'aurelia-framework';

import * as LogManager from 'aurelia-logging'
var log = LogManager.getLogger('shell');

@autoinject()
export class Shell {

    private _router: Router;

    configureRouter(config: RouterConfiguration, router: Router): void {
        this._router = router;
        config.title = 'ArmChair - Todo';

        config.map([
            { route: ['', 'active', 'all'], name: 'app', moduleId: 'app' }
        ]);
    }

}
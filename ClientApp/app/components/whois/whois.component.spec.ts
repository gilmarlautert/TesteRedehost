/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { assert } from 'chai';
import { WhoisComponent } from './whois.component';
import { TestBed, async, ComponentFixture } from '@angular/core/testing';

let fixture: ComponentFixture<WhoisComponent>;

describe('Counter component', () => {
    beforeEach(() => {
        TestBed.configureTestingModule({ declarations: [WhoisComponent] });
        fixture = TestBed.createComponent(WhoisComponent);
        fixture.detectChanges();
    });

    it('should display a title', async(() => {
        const titleText = fixture.nativeElement.querySelector('h1').textContent;
        expect(titleText).toEqual('Whois');
    }));

    it('should start with disable button pesquisar', async(() => {
        const countElement = fixture.nativeElement.querySelector('button.btn.btn-default');
        expect(countElement.textContent).toEqual('Pesquisar');
        expect(countElement.disable).toEqual(true);

        const incrementButton = fixture.nativeElement.querySelector('button');
        incrementButton.click();
        fixture.detectChanges();
        expect(countElement.textContent).toEqual('1');
    }));

    it('should start with disable button pesquisar', async(() => {
        const countElement = fixture.nativeElement.querySelector('button.btn.btn-default');
        expect(countElement.textContent).toEqual('Pesquisar');
        expect(countElement.disable).toEqual(true);
    }));

    
    it('should enable button pesquisar, when input text greather then 0', async(() => {
        const countElement = fixture.nativeElement.querySelector('button.btn.btn-default');
        expect(countElement.textContent).toEqual('Pesquisar');
        expect(countElement.disable).toEqual(true);

        const inputElement = fixture.nativeElement.querySelector('input#text');
        inputElement.value = ".com";
        fixture.detectChanges();
        expect(countElement.disable).toEqual(false);
    }));    
});

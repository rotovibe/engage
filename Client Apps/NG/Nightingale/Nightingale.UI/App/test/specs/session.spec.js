define(['services/session'], function (session) {
    describe('Session is where our user information is stored.', function(){

    	it("should create an empty user object", function () {
			expect(session.currentUser()).toBe(undefined);
		});
    	it("should check for localStorage and cookies", function () {
			expect(session.runTests()).toMatch({ ls: true, cookies: true });
		});
        it('clear session', function () {
            expect(true).toBe(true);
        });
    });
});
let tl = gsap.timeline();

tl.from(".avartar-card", {
  stagger: 0.2,
  duration: 0.3,
  opacity: 0,
  x: -20,
});

tl.from(".avatar img", {
  stagger: 0.2,
  duration: 0.3,
  opacity: 0,
  y: 20,
});

tl.from(".user-online-indicator", {
  stagger: 0.2,
  duration: 0.3,
  opacity: 0,
  y: 20,
})
tl.from(".profile-name", {
  stagger: 0.2,
  duration: 0.3,
  opacity: 0,
  y: 20,
})
tl.from(".profile-role", {
  stagger: 0.2,
  duration: 0.3,
  opacity: 0,
  y: 20,
})
tl.from("#fh5co-main-menu li", {
  stagger: 0.2,
  duration: 0.3,
  opacity: 0,
  y: 20,
})

